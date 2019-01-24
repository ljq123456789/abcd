
use Month11
--员工表
drop table Manage
create table Manage
(
Id int primary key identity, --自增主键ID
WId int,                 --工号
Pwid int,                --登录密码
Name varchar(50),        --姓名
RoleName varchar(50),    --职位名称
Depart varchar(50),      --部门名称
PorwerId int             --权限id 0:无审核权限，1：本部门，2：全部
)
insert into Manage values('1111','1111','周星星','普通员工','技术部',0),
('1112','1112','小秋','普通员工','设计部',0),('1113','1113','老王','技术部经理','技术部',1),
('1114','1114','李总','公司总经理','总公司',2)
select *from Manage

--请假表
create table Leave
(
Id int primary key identity,  --自增主键ID
ManageId int,           --绑定员工表
Start datetime,         --请假开始时间
Ends datetime,          --请假结束时间
Reason varchar(50),     --是由
Ramark varchar(500),    --备注
Statio int,             --0:未提交，1：待审核(部门)，2：待审核(总经理)，3：已通过，4：已驳回 
Qing datetime,           --申请时间
Shen datetime,          --审核时间
Bo varchar(100)         --驳回理由
)

select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where ManageId=1 order by Qing desc

select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where Start>'2019-01-01' and Ends <'2019-01-17' and ManageId= '1' order by Qing desc

drop table Leave

create table Checked
(
Id int primary key identity,
Mid int,
Lid int,
Shen datetime,          --审核时间
Bo varchar(100)         --驳回理由
)

--请假图片
create table Img
(
Id int primary key identity,
Picture varchar(100),  --图片路径
Tid int,               --图片父级表 0:用户，1:请假
Pid int,               --关联编号
)
select *from Img

create proc Manage_login
(@WId int,@Pwid int)
as begin
  select *from Manage where WId=@WId and Pwid=@Pwid
end


insert into Members values('dd','女','两年',GETDATE())

select *from Member



