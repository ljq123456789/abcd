
use Month11
--Ա����
drop table Manage
create table Manage
(
Id int primary key identity, --��������ID
WId int,                 --����
Pwid int,                --��¼����
Name varchar(50),        --����
RoleName varchar(50),    --ְλ����
Depart varchar(50),      --��������
PorwerId int             --Ȩ��id 0:�����Ȩ�ޣ�1�������ţ�2��ȫ��
)
insert into Manage values('1111','1111','������','��ͨԱ��','������',0),
('1112','1112','С��','��ͨԱ��','��Ʋ�',0),('1113','1113','����','����������','������',1),
('1114','1114','����','��˾�ܾ���','�ܹ�˾',2)
select *from Manage

--��ٱ�
create table Leave
(
Id int primary key identity,  --��������ID
ManageId int,           --��Ա����
Start datetime,         --��ٿ�ʼʱ��
Ends datetime,          --��ٽ���ʱ��
Reason varchar(50),     --����
Ramark varchar(500),    --��ע
Statio int,             --0:δ�ύ��1�������(����)��2�������(�ܾ���)��3����ͨ����4���Ѳ��� 
Qing datetime,           --����ʱ��
Shen datetime,          --���ʱ��
Bo varchar(100)         --��������
)

select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where ManageId=1 order by Qing desc

select Leave.Statio,Manage.Depart,Manage.WId,Manage.Name,Leave.Qing,Leave.Shen,Leave.Bo from Leave join Manage on Leave.ManageId = Manage.Id where Start>'2019-01-01' and Ends <'2019-01-17' and ManageId= '1' order by Qing desc

drop table Leave

create table Checked
(
Id int primary key identity,
Mid int,
Lid int,
Shen datetime,          --���ʱ��
Bo varchar(100)         --��������
)

--���ͼƬ
create table Img
(
Id int primary key identity,
Picture varchar(100),  --ͼƬ·��
Tid int,               --ͼƬ������ 0:�û���1:���
Pid int,               --�������
)
select *from Img

create proc Manage_login
(@WId int,@Pwid int)
as begin
  select *from Manage where WId=@WId and Pwid=@Pwid
end


insert into Members values('dd','Ů','����',GETDATE())

select *from Member



