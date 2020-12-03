
## Inroduction：
This solution  aims to bulid  a full set of  enterprise management system SAAS architecture.  including 4 projects: Ligg.Abp, Ligg.Vue, Ligg.Ewa, Ligg.Mvc. Following is the a diagram of their structure.
![diagram of their structure](https://gitee.com/uploads/images/2018/0328/150758_26ef9d61_362401.png "diagram of their structure")

## Ligg.Abp
 Ligg.Abp is the main server end connected to main database to provide Restful interface to Ligg.Vue, Ligg.Ewa, Ligg.Mvc, based on ABP vNext 2.5 (. Net core 3.1, webapi,  JWT, Autofac, EF(code first), swagger ...);

## Ligg.Vue
Ligg.Vue is the main front end presentation tool , based on Vue 2.6, integrated into  elementUI, ztree, echarts and Visual process design tool-jsplumb ; 

## Ligg.Ewa
Ligg.Ewa is the web front end Configured from  Ligg.EasyWinApp, used as system configuration, db initialization, pprogram testing (both press and function); 

## Ligg.Mvc
Ligg.Mvc based on Asp.net core 3.1 MVC,  tech stack including incluning EF core(db first), autofac, quartz, bootstrap, etc . Ligg.Mvc will be used as CMS and portal generator for responsive web page, or mobile-end H5 page for the useage of  mobile user,  for example, order approval, survey, etc.  

##Thanks
The codes  are referencing the work of others,  meowv/blog, PanJiaChen/vue-element-admin and other open-source contributors.  Thank you all.


E:/Doing/coding/git/Ligg.Farch


