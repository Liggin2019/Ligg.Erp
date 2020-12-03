## Inroduction：
This solution  aims to bulid  a full set of  enterprise management system SAAS architecture ( for OA, ERP, HRMS, MES, etc).  including 4 projects: Ligg.Abp, Ligg.Vue, Ligg.Ewa, Ligg.Mvc. Following is the diagram of their structure.

## Ligg.Abp
 Ligg.Abp is the main server end connected to main database to provide Restful interface to Ligg.Vue, Ligg.Ewa, Ligg.Mvc, based on ABP vNext 2.5(. Net core 3.1, webapi, mutiple tenants ) inluding  JWT, Autofac, Hangfire, EF(code first), swagger;

## Ligg.Vue
Ligg.Vue is the main front end presentation UI , based on Vue 2.6, integrated into  elementUI, ztree, echarts and Visual process design tool-jsplumb; 

## Ligg.Ewa
Ligg.Ewa is a Winform web front end Configured from  Ligg.EasyWinApp, used as system configuration, db initialization, program testing (both press and function); also it can be used for MES as a bridge between device and Ligg.Abp, because it is easy to connect device. 

## Ligg.Mvc
Ligg.Mvc based on Asp.net core 3.1 MVC,  tech stack including EF core(db first), autofac, quartz, bootstrap, etc. Ligg.Mvc will be used as CMS and portal generator for responsive web page, or mobile-end H5 page for the usage in  mobile environment. For example, order approval, survey, official site and intranet portals.  

## Thanks
The codes  reference the work of meowv/blog, PanJiaChen/vue-element-admin and other open-source contributors.  Thank you all.


