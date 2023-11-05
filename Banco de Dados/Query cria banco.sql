go
create database MoneyMind

go
use MoneyMind
------------------------------------------Criação da tabela Usuario---------------------------------------------------
go
create table Usuario
(
	id_usuario int primary key identity(1,1) not null,
	login_usuario varchar(100) not null,
	nome_pessoa varchar(100) not null,
	senha varchar(100) not null,
	imagem varbinary(max) null,
	adm bit default(0)

	UNIQUE (login_usuario)
)
------------------------------------------Criação da tabela Portifolio------------------------------------------------
go
create table Portifolio
(
	id_portifolio int primary key identity(1,1) not null,
	nome varchar(100) not null,
	id_usuario int not null,
	
	foreign key (id_usuario) references Usuario(id_usuario),

	UNIQUE (id_usuario)
)
------------------------------------------Criação da tabela Carteira--------------------------------------------------
go
create table Carteira
(
	id_carteira int primary key identity(1,1) not null,
	descricao varchar(100) null,
	id_portifolio int not null,
	foreign key (id_portifolio) references Portifolio(id_portifolio)
)

------------------------------------------Criação da Tabela Geral-----------------------------------------------------

go
create table Tabela_Geral
(
	id_tabela_geral int primary key identity(1,1) not null,
	descricao varchar(100) not null,
)

go
insert into Tabela_Geral values ('TG_OPERACAO')
insert into Tabela_Geral values ('TG_CLASSE_ATIVO')

------------------------------------------Criação da tabela Item Geral------------------------------------------------
go
create table Item_Tabela_Geral
(
	id_item_tabela_geral int primary key identity(1,1) not null,
	id_tabela_geral int not null,
	descricao varchar(100) not null,

	foreign key (id_tabela_geral) references Tabela_Geral(id_tabela_geral),
)

go
insert into Item_Tabela_Geral values ( 1, 'COMPRA')
insert into Item_Tabela_Geral values ( 1, 'VENDA')
insert into Item_Tabela_Geral values ( 2, 'ACAO')
insert into Item_Tabela_Geral values ( 2, 'FUNDO IMOBILIARIO')
insert into Item_Tabela_Geral values ( 2, 'TESOURO DIRETO')
insert into Item_Tabela_Geral values ( 2, 'CDB')
insert into Item_Tabela_Geral values ( 2, 'CRI')

--------------------------------------------------Criação da tabela Ativo--------------------------------------------------
go
create table Ativo
(
	id_ativo int primary key identity(1,1) not null,
	ticker varchar(20) not null,
	empresa varchar(100) not null,
	id_classe_ativo int not null,
	
	foreign key (id_classe_ativo) references Item_Tabela_Geral(id_item_tabela_geral)
)
------------------------------------------Criação da tabela Movimentação------------------------------------------------
go
create table Movimentacao
(
	id_movimentacao int primary key identity(1,1) not null,
	id_carteira int not null,
	id_ativo int not null,
	id_operacao int not null,
	quantidade int not null,
	preco decimal (10,2) not null,
	data_hora_movimentacao datetime not null,
	
	foreign key (id_carteira) references Portifolio(id_portifolio),
	foreign key (id_ativo) references Ativo(id_ativo),
	foreign key (id_operacao) references Item_Tabela_Geral(id_item_tabela_geral),
)

go
insert into Usuario values ('pauloht', 'Paulo Henrique Tristão', 'moneymindAdm@', null, 1)
insert into Usuario values ('Puertas', 'Rodrigo Puertas Matioli', 'moneymindAdm@', null, 1)
go


--------------------------------------------------SP's ATIVO-------------------------------------------------------
GO
create or alter procedure spInsert_Ativo
 @id int,
 @ticker varchar(max),
 @empresa varchar(max),
 @id_classe_ativo int
as
begin
 insert into Ativo(ticker, empresa, id_classe_ativo)
 values (@ticker, @empresa, @id_classe_ativo)
end
GO


create or alter procedure spExclui_Ativo
(
 @id int
)
as
begin
 delete Ativo where id_ativo = @id
end
GO

create or alter procedure spUpdate_Ativo
(
 @id int,
 @ticker varchar(max),
 @empresa varchar(max),
 @id_classe_ativo int
)
as
begin
 update Ativo set ticker = @ticker, empresa = @empresa, id_classe_ativo = @id_classe_ativo where id_ativo = @id
end
GO
------------------------------------------------------SP's GENERICAS-----------------------------------------------
create or alter procedure spListagem
(
 @tabela varchar(max),
 @ordem varchar(max))
as
begin
 exec('select * from ' + @tabela +
 ' order by ' + @ordem)
end
GO

create or alter procedure spConsulta
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = 'select * from ' + @tabela +
 ' where id = ' + cast(@id as varchar(max))
 exec(@sql)
 end
GO

-----------------------------------------------------SP's USUARIO--------------------------------------------------

go
Create or alter procedure spUpdate_Usuario
(
	@id int,
	@login_usuario varchar(max),
	@nome_pessoa varchar(max),
	@senha varchar(max),
	@adm bit,
	@imagem varbinary(max)
)
as
begin
	update Usuario set login_usuario = @login_usuario, nome_pessoa = @nome_pessoa,
					   senha = @senha, adm = @adm, imagem = @imagem 
					   where id_usuario = @id
end

go
create or alter procedure spInsert_Usuario
(
	@id int,
	@login_usuario varchar(max),
	@nome_pessoa varchar(max),
	@senha varchar(max),
	@adm bit,
	@imagem varbinary(max)
)
as
begin
	insert into Usuario (id_usuario, login_usuario, nome_pessoa, senha, adm, imagem) 
			            values (@id, @login_usuario, @nome_pessoa, @senha, @adm, @imagem)
end

go
create or alter procedure spExclui_Usuario
(
	@id int
)
as
begin
 delete Usuario where id_usuario = @id
end
GO


------------------------------------------------SP's Login---------------------------------------------------------
GO
create or Alter procedure spConsulta_Acesso
(
	@login_usuario varchar(max),
	@senha varchar(max)
)
as
begin
	select id_usuario, login_usuario, nome_pessoa, senha, imagem, adm from Usuario where login_usuario = @login_usuario and senha = @senha
end
GO



------------------------------------------------SP's Portifolio---------------------------------------------------------

go
create or alter procedure spConsulta_PortifolioPorUsuario
(
   @id_usuario int
)
as
begin
 select * from Portifolio where id_usuario = @id_usuario
end
GO

------------------------------------------------SP's Carteira---------------------------------------------------------

create or alter procedure spConsulta_CarteiraPorPortifolio
(
   @id_portifolio int
)
as
begin
 select * from Carteira where id_portifolio = @id_portifolio
end
GO

------------------------------------------------SP's Movimentacao---------------------------------------------------------
create or alter procedure spConsulta_MovimentacaoPorCarteira
(
   @id_carteira int
)
as
begin
 select * from Movimentacao where id_carteira = @id_carteira
end
GO