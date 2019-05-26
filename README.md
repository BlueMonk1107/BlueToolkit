# BlueToolkit
A set of tools designed to increase the efficiency of unity development.

## 工程目录
**AutoAddNamespace：** Auto add namespace（自动添加命名空间工具） 

**AutoGenerateECSFrameCode：** Auto create Entitas script and initialize system（自动创建Entitas框架配套代码及自动初始化System）    

**AutoGenerateStrangeIOCCode：** Auto create StrangeIOC script（自动创建StrangeIOC框架配套代码）    

**ExportObj：** Export obj from Unity(Unity导出模型插件)     
  
**FontTool：** Set the text's default font of UGUI (设置UI字体插件)    

**Common：** The common scripts in the toolkit (插件工具类)    

**TextureSetting：** Auto set the parameter when import texture (导入图片自动设置格式插件)    

**UnityExpand：** The Unity expand class (拓展类脚本目录)      

**Shader：** Shader目录   

## 已有内容
### 拓展类：     
&ensp;&ensp;Transform拓展，详见TransformExpand类     
&ensp;&ensp;UI拓展，详见UIExpand类  
&ensp;&ensp;Animator拓展，详见AnimatorUtil类   
### 工具类：  
&ensp;&ensp;1）设置Text默认字体     
&ensp;&ensp;2）一键统一修改目录下所有预制体的Text字体插件      
&ensp;&ensp;3）导入图片自动设置格式，详见TextureImportSetting   
&ensp;&ensp;4）自动添加命名空间，采用类似java的包名方式，通过文件所在的文件夹对命名空间命名       
&ensp;&ensp;5）用于StrangeIOC自动生成框架脚本的工具      
&ensp;&ensp;6）用于Entitas自动生成框架脚本及自动初始化系统的工具    
&ensp;&ensp;7）Unity导出模型插件       
### Shader：
&ensp;&ensp;**MaskIcon：** 使用UI自带Mask切圆形锯齿严重，这个Shader可以消除锯齿   
