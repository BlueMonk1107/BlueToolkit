using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WordShowEffect : BaseMeshEffect
{
    public enum WordEffect
    {
        NULL,
        WHOLE_WORD,
        LEFT_TO_RIGHT
    }

    [Tooltip("每个字的动画执行时间")]
    public float EachEffectTime = 0.5f;
    [Tooltip("每隔多久刷新一次mesh")]
    public float UpdateMeshTime = 0.1f;
    [Tooltip("选择动画模式")]
    public WordEffect WordEffectMode;
    //动画执行到第几个字的标记，第一个字的标记是1
    private int _currentWordIndex = 1;
    //开始当前字的动画的时间
    private float _updateWordTime = 0;
    /// <summary>
    /// 是否正在执行动画
    /// </summary>
    public bool IsRunning { get; private set; }

    //要显示的Text组件
    private Text _text;
    //当前刷新次数
    private int _updateNum;
    //记录之前的文本
    private string _oldText;
    //每个字的更新次数
    private int _wordUpdateTimes;
    private float _alphaOffset;
    private WordAlphaEffectBase _wordAlphaEffect;
    private Action _onComplete;

    //初始化信息
    protected override void Awake()
    {
        InitData();
        _text = GetComponent<Text>();
    }

    void Update()
    {
        if (IsRunning)
        {
            if (Time.time - _updateWordTime > UpdateMeshTime)
            {
                _updateWordTime = Time.time;
                if (_updateNum == _wordUpdateTimes)
                {
                    if (_currentWordIndex < _text.text.Length)
                    {
                        _currentWordIndex++;
                        _updateNum = 0;
                    }
                    else
                    {
                        IsRunning = false;
                        if (_onComplete != null)
                            _onComplete();
                        Debug.Log("end");
                    }
                }
                _updateNum++;
                graphic.SetVerticesDirty();
            }
        }
        Debug.Log("isRuning:"+IsRunning);
    }
    
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;
        
        if (_oldText != _text.text)
        {
            _oldText = _text.text;
            Run();
        }
        
        if(_wordAlphaEffect == null)
            return;

        _wordAlphaEffect.ModifyMesh(vh,_currentWordIndex,_updateNum);
    }

    private void InitData()
    {
        _updateWordTime = Time.time;
        _currentWordIndex = 1;
        _updateNum = 0;
        _wordUpdateTimes = Mathf.CeilToInt(EachEffectTime / UpdateMeshTime);
        _alphaOffset = 1.0f / _wordUpdateTimes;
        InitEffectMode();
        if(_wordAlphaEffect != null)
            _wordAlphaEffect.Init(_alphaOffset,_wordUpdateTimes);
    }

    private void InitEffectMode()
    {
        switch (WordEffectMode)
        {
            case WordEffect.NULL:
                _wordAlphaEffect = new NullEffect();
                break;
            case WordEffect.WHOLE_WORD:
                _wordAlphaEffect = new ChangeWholeWordAlpha();
                break;
            case WordEffect.LEFT_TO_RIGHT:
                _wordAlphaEffect = new LeftToRightAlpha();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// 添加动画完成监听，主动停止的动画也会调用回调
    /// </summary>
    /// <param name="complete"></param>
    public void AddCompleteListener(Action complete)
    {
        _onComplete = complete;
    }

    /// <summary>
    /// 选择当前动画模式
    /// </summary>
    /// <param name="mode"></param>
    public void ChangeEffectMode(WordEffect mode)
    {
        WordEffectMode = mode;
        InitData();
    }

    /// <summary>
    /// 修改当前显示内容，可设置延时时间，控制新内容的显示时机
    /// </summary>
    /// <param name="content"></param>
    /// <param name="delayTime"></param>
    public void ChangeContent(string content,float delayTime = 1)
    {
        Stop();
        StopAllCoroutines();
        StartCoroutine(Wait(content,delayTime));
    }

    //避免unity版本问题，没有使用C#异步
    private IEnumerator Wait(string content,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _text.text = content;
    }
    
    /// <summary>
    /// 运行当前动画，可重新运行动画
    /// </summary>
    public void Run()
    {
        InitData();
        IsRunning = WordEffectMode != WordEffect.NULL;
    }
    
    /// <summary>
    /// 停止当前动画，直接显示最后结果
    /// </summary>
    public void Stop()
    {
        _currentWordIndex = _text.text.Length;
        _updateNum = _wordUpdateTimes;
    }
}

public abstract class WordAlphaEffectBase
{
    protected int _wordUpdateTimes;
    protected float _alphaOffset; 
    public void Init(float alphaOffset, int wordUpdateTimes)
    {
        _wordUpdateTimes = wordUpdateTimes;
        _alphaOffset = alphaOffset;
    }

    public abstract void ModifyMesh(VertexHelper vh, int currentWordIndex, int updateNum);
    protected abstract void SetColor(List<UIVertex> verts, int currentWordIndex, int updateNum);
}
public class NullEffect : WordAlphaEffectBase
{

    public override void ModifyMesh(VertexHelper vh,int currentWordIndex,int updateNum)
    {
        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);
        vh.Clear();
        
        if(verts.Count == 0)
            return;

        int index = verts.Count / 6;

        AddVert(vh, verts, index * 6);
        AddTriangle(vh, index*4);
    }

    protected override void SetColor(List<UIVertex> verts, int currentWordIndex, int updateNum)
    {
        
    }

    private void AddVert(VertexHelper vh,List<UIVertex> verts,int count)
    {
        for (int i = 0; i < count; i+=6)
        {
            var tl = verts[i + 0];
            var tr = verts[i + 1];
            var bl = verts[i + 4];
            var br = verts[i + 3];

            vh.AddVert (tl);
            vh.AddVert (tr);
            vh.AddVert (bl);
            vh.AddVert (br);
        }
    }

    private void AddTriangle(VertexHelper vh,int count)
    {
        for (int i = 0; i < count; i += 4) {
            vh.AddTriangle (i + 0, i + 1, i + 2);
            vh.AddTriangle (i + 1, i + 3, i + 2);
        }
    }
}

public class ChangeWholeWordAlpha : WordAlphaEffectBase
{
    protected override void SetColor(List<UIVertex> verts,int currentWordIndex,int updateNum)
    {
        for (int i = (currentWordIndex - 1)*6; i < currentWordIndex*6 && i<verts.Count; i++)
        {
            UIVertex vertex = verts[i];
            Color temp = vertex.color;
            temp.a = 0;
            temp.a += _alphaOffset * updateNum;
            vertex.color = temp;
            verts[i] = vertex;
        }
        
    }
    
    public override void ModifyMesh(VertexHelper vh,int currentWordIndex,int updateNum)
    {
        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);
        vh.Clear();
        SetColor(verts,currentWordIndex,updateNum);

        AddVert(vh, verts, currentWordIndex * 6);
        AddTriangle(vh, currentWordIndex*4);
    }
    
    private void AddVert(VertexHelper vh,List<UIVertex> verts,int count)
    {
        for (int i = 0; i < count; i+=6)
        {
            var tl = verts[i + 0];
            var tr = verts[i + 1];
            var bl = verts[i + 4];
            var br = verts[i + 3];

            vh.AddVert (tl);
            vh.AddVert (tr);
            vh.AddVert (bl);
            vh.AddVert (br);
        }
    }

    private void AddTriangle(VertexHelper vh,int count)
    {
        for (int i = 0; i < count; i += 4) {
            vh.AddTriangle (i + 0, i + 1, i + 2);
            vh.AddTriangle (i + 1, i + 3, i + 2);
        }
    }
}
public class LeftToRightAlpha : WordAlphaEffectBase
{
    public override void ModifyMesh(VertexHelper vh, int currentWordIndex, int updateNum)
    {
        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);
        vh.Clear();

        if (verts.Count == 0)
            return;
        int count = currentWordIndex * 6;
        
        SetColor(verts,currentWordIndex,updateNum);
        AddVert(vh, verts, count);
        AddTriangle(vh, count);
    }

    private void AddVert(VertexHelper vh,List<UIVertex> verts,int count)
    {
        for (int i = 0; i < count; i+=6)
        {
            var tl = verts[i + 0];
            var tr = verts[i + 1];
            var bl = verts[i + 4];
            var br = verts[i + 3];
            var ct = GetCenterVertex(verts[i + 0], verts[i + 1]);
            var cb = GetCenterVertex(verts[i + 3], verts[i + 4]);

            vh.AddVert (tl);
            vh.AddVert (tr);
            vh.AddVert (bl);
            vh.AddVert (br);
            vh.AddVert (ct);
            vh.AddVert (cb);
        }

    }

    private void AddTriangle(VertexHelper vh,int count)
    {
        for (int i = 0; i < count; i += 6) {
            vh.AddTriangle (i + 0, i + 4, i + 2);
            vh.AddTriangle (i + 4, i + 5, i + 2);
            vh.AddTriangle (i + 4, i + 1, i + 5);
            vh.AddTriangle (i + 1, i + 3, i + 5);
        }
    }

    protected override void SetColor(List<UIVertex> verts,int currentWordIndex,int updateNum)
    {
        int halfTimes = _wordUpdateTimes*2 / 3;
        currentWordIndex = (currentWordIndex - 1)*6;
        if (updateNum < halfTimes)
        {
            //last
            SetColor(verts, currentWordIndex+1-6, _alphaOffset, updateNum+halfTimes);
            SetColor(verts, currentWordIndex+3-6, _alphaOffset, updateNum+halfTimes);
            
            //hide
            SetColor(verts, currentWordIndex+1, 0, updateNum);
            SetColor(verts, currentWordIndex+3, 0, updateNum);
            
            //show
            SetColor(verts, currentWordIndex+0, _alphaOffset, updateNum);
            SetColor(verts, currentWordIndex+4, _alphaOffset, updateNum);

            
        }
        else
        {
            SetColor(verts, currentWordIndex+1, _alphaOffset, updateNum-halfTimes);
            SetColor(verts, currentWordIndex+3, _alphaOffset, updateNum-halfTimes);
        }
    }

    private void SetColor(List<UIVertex> verts,int index,float alphaOffset,int updateNum)
    {
        if(index <0 || index >= verts.Count)
            return;
        UIVertex vertex = verts[index];
        Color temp = vertex.color;
        temp.a = 0;
        temp.a += alphaOffset * updateNum;
        vertex.color = temp;
        verts[index] = vertex;
    }
    
    private UIVertex GetCenterVertex(UIVertex left, UIVertex right)
    {
        UIVertex center = new UIVertex();
        center.normal = (left.normal + right.normal) / 2;
        center.position = (left.position + right.position) / 2;
        center.tangent = (left.tangent + right.tangent) / 2;
        center.uv0 = (left.uv0 + right.uv0) / 2;
        center.uv1 = (left.uv1 + right.uv1) / 2;

        var color = Color.Lerp(left.color, right.color, 0.5f);
        center.color = color;
        return center;
    }
}