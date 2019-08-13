use ACM_Studio_V2
go

create table UnitTestCustomer
(
    customerIid int primary key not null identity (1,1),
    Name varchar(10),
    TotalPurchases money,
    TotalReturns money
)
go

create procedure UnitTestCreateCustomer
    @name varchar(200),
    @totalPurchases money,
    @totalReturns money
as
insert into UnitTestCustomer
values (@name, @totalPurchases, @totalReturns)

go        