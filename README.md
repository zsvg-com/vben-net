### 🍿 项目简介

[![码云Gitee](https://gitee.com/vben/vben-net/badge/star.svg?theme=blue)](https://gitee.com/vben/vben-net)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)]()

vben-net 是一款基于.NET8（.NET10）+Vue3的前后端分离快速开发框架。

后端主要参考了 [Admin.Net](https://gitee.com/zuohuaijun/Admin.NET) 与 [ZRAdmin](https://gitee.com/izory/ZrAdminNetCore)，集成业界一流技术栈，针对企业痛点，具有组件化、模块化、轻耦合、高扩展等特色。
设计上兼容了Furion框架与原生ASP.NET Core框架，可自由切换。

前端基于 [Vben Admin](https://github.com/vbenjs/vue-vben-admin) 改造，使用最新前端技术栈，提供丰富的组件和模板以及N种偏好设置组合方案，
应用层可自由选择UI框架（Element Plus，Ant Design Vue，Native UI）

移动端基于 [cool-unix](https://gitee.com/cool-team-official/cool-unix) 改造，参照了unibest的方式，通过pnpm集成，
除了可继续使用HBuilderX，还可以使用VS Code，Webstorm 开发运行uni-app x 应用（WEB与小程序）。注：安卓、苹果、鸿蒙端的运行还是需要借助HBuilderX。

### 🍟 相关地址

* 前端WEB 体验地址 ：[http://8.153.168.178/](http://8.153.168.178/)

* 移动端APP 体验地址 ：[http://8.153.168.178/app/](http://8.153.168.178/app/)

* 文档地址 ：[http://8.153.168.178/net-doc](http://8.153.168.178/net-doc)

* 后端API 项目地址 https://gitee.com/vben/vben-net

* 前端WEB 项目地址 https://gitee.com/vben/vben-web

* 移动端APP 项目地址 https://gitee.com/vben/vben-app

* 同功能java后端 项目地址 https://gitee.com/vben/vben-java

* 联系方式（加微信后可找小狐狸拉进交流群）：

![输入图片说明](https://gitee.com/vben/vben-app/raw/master/docs/wx.jpg "微信联系方式")

### ⚡ 快速启动

后端

* 准备工作：1. 准备.NET8（.NET10）环境，修改根目录下的Directory.Build.props文件中对应的全局TargetFramework 2. 根据Vben.Admin应用下/Properties/Configs/web.Development.json配置文件准备一个空的数据库，默认为vben-net的mysql数据库，账号root,密码123456 3. 开启redis 默认密码为空

* 启动后台API：启动VbenAdmin，系统会根据SqlSugar CodeFirst自动生成数据库表结构，另外会根据Init相关服务生成数据库初始化数据。也可启动VbenDemo查看相应DEMO示例

前端

* 准备工作：准备Node.js 20.15.0以上环境，全局安装pnpm：npm install -g pnpm

* 启动前台WEB：1. 使用pnpm install安装依赖 2. 使用pnpm dev:ele运行项目 3.访问 http://localhost:5666/ 预览

### 🍄 主要特色

- 后端项目中抽离并拆分了公共功能，以插件化与扩展包的方式组织，结构解耦且易于扩展。
- 后端项目业务模块以多基础模块与多应用方式组织，可实现多个应用共用相同基础模块，方便实现基础模块共享
- 后端项目设计上兼容了Furion框架与原生ASP.NET core框架，可自由切换。
- 工作流模块不依赖其他工作流引擎，全部自行实现，易于扩展，实现复杂工作流
- 统一的命名风格，数据库表主键统一用ID命名，表字段采用SAP风格（后端手册里有详细介绍）


### 🍖 内置功能

以下表格列出了Furion版本（master分支）与Native版本(native分支)的差异。

<table>
    <thead>
    <tr>
        <th width="150" align="center">业务模块</th>
        <th width="*">功能说明</th>
        <th width="150" align="center">Furion版本</th>
        <th width="150" align="center">Native版本</th>
    </tr>
    </thead>
    <tbody>
    <tr>
        <td align="center">用户管理</td>
        <td>用户的管理配置 如:新增用户、分配用户所属部门、角色、岗位等</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">部门管理</td>
        <td>配置系统组织机构（公司、部门、小组） 树结构展现支持数据权限</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">岗位管理</td>
        <td>配置系统用户所属担任职务</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">菜单管理</td>
        <td>配置系统菜单、操作权限、按钮权限标识等</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">角色管理</td>
        <td>角色根据部门、用户、岗位、群组分配权限</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">字典管理</td>
        <td>对系统中经常使用的一些较为固定的数据进行维护</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">参数管理</td>
        <td>对系统动态配置常用参数</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">通知公告</td>
        <td>系统通知公告信息发布维护</td>
        <td align="center">√</td>
        <td align="center">× 待支持</td>
    </tr>
    <tr>
        <td align="center">客户端管理</td>
        <td>系统内对接的所有客户端管理 如: pc端、小程序端等，支持动态授权登录方式 如: 短信登录、密码登录等 支持动态控制token时效</td>
        <td align="center">× 待支持</td>
        <td align="center">× 待支持</td>
    </tr>
    <tr>
        <td align="center">操作日志</td>
        <td>系统正常操作日志记录和查询 系统异常信息日志记录和查询</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">登录日志</td>
        <td>系统登录日志记录查询包含登录异常</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">文件管理</td>
        <td>同时支持本地文件存储于分布式对象存储。系统文件展示、上传、下载、删除等管理</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">在线用户管理</td>
        <td>已登录系统的在线用户信息监控与强制踢出操作</td>
        <td align="center">√</td>
        <td align="center">× 待支持</td>
    </tr>
    <tr>
        <td align="center">定时任务</td>
        <td>运行报表、任务管理(添加、修改、删除)、日志管理、执行器管理等</td>
        <td align="center">√</td>
        <td align="center">×</td>
    </tr>
    <tr>
        <td align="center">代码生成</td>
        <td>前后端代码的生成（c#、vue、sql）支持CRUD下载</td>
        <td align="center">√</td>
        <td align="center">×</td>
    </tr>
    <tr>
        <td align="center">系统接口</td>
        <td>根据业务代码自动生成相关的api接口文档</td>
        <td align="center">√ 全面</td>
        <td align="center">√ 基本</td>
    </tr>
    <tr>
        <td align="center">服务监控</td>
        <td>监视系统CPU、内存、磁盘、堆栈、在线日志等</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">缓存监控</td>
        <td>对系统的缓存信息查询，命令统计等</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">在线构建器</td>
        <td>拖动表单元素生成相应的HTML代码</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">使用案例</td>
        <td>系统的一些功能案例</td>
        <td align="center">√</td>
        <td align="center">√</td>
    </tr>
    <tr>
        <td align="center">工作流</td>
        <td>前端流程图采用BPMN.js，表单采用FromCreate（FormDesigner），后端自行实现流程引擎</td>
        <td align="center">× 待java版本稳定</td>
        <td align="center">× 待java版本稳定</td>
    </tr>
    </tbody>
</table>

### 💐 特别鸣谢
- 👉 原框架作者：[zsvg](https://gitee.com/zsvg)
- 👉 Vben-Admin：[https://github.com/vbenjs/vue-vben-admin](https://github.com/vbenjs/vue-vben-admin)
- 👉 Furion：[https://furion.net/](https://furion.net/)
- 👉 SqlSugar：[https://www.donet5.com/Home/Doc](https://www.donet5.com/Home/Doc)
- 👉 Admin.Net：[https://gitee.com/zuohuaijun/Admin.NET](https://gitee.com/zuohuaijun/Admin.NET)
- 👉 ZrAdmin：[https://gitee.com/izory/ZrAdminNetCore](https://gitee.com/izory/ZrAdminNetCore)

