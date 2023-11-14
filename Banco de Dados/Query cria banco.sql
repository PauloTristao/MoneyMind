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
	
	foreign key (id_usuario) references Usuario(id_usuario) on delete cascade,

	UNIQUE (id_usuario)
)
------------------------------------------Criação da tabela Carteira--------------------------------------------------
go
create table Carteira
(
	id_carteira int primary key identity(1,1) not null,
	descricao varchar(100) null,
	id_portifolio int not null,
	foreign key (id_portifolio) references Portifolio(id_portifolio) on delete cascade
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


go
create or ALTER procedure [dbo].[spListagemAtivo]
(
   @ativo varchar(max)
)
as
begin
	select * from Ativo
end
go
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
 ' where id_' + @tabela + ' = ' + cast(@id as varchar(max))
 exec(@sql)
 end
GO

CREATE OR ALTER procedure spDelete
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = ' delete ' + @tabela +
 ' where id_' + @tabela + ' = ' + cast(@id as varchar(max))
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
	insert into Usuario (login_usuario, nome_pessoa, senha, adm, imagem) 
			            values (@login_usuario, @nome_pessoa, @senha, @adm, @imagem)
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
Create or alter procedure spUpdate_Portifolio
(
	@id int,
	@Nome varchar(max),
	@id_usuario int
)
as
begin
	update Portifolio set nome = @Nome, id_usuario = @id_usuario
					   where id_usuario = @id
end

go
create or alter procedure spInsert_Portifolio
(
	@id int,
	@Nome varchar(max),
	@id_usuario int
)
as
begin
	insert into Portifolio (nome, id_usuario) 
			            values (@nome, @id_usuario)
end

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

go
Create or alter procedure spUpdate_Carteira
(
	@id int,
	@descricao varchar(max),
	@id_portifolio int
)
as
begin
	update Carteira set descricao = @descricao, id_portifolio = @id_portifolio
					   where id_carteira = @id
end

go
create or alter procedure spInsert_Carteira
(
	@id int,
	@descricao varchar(max),
	@id_portifolio int
)
as
begin
	insert into Carteira (descricao, id_portifolio) 
			            values (@descricao, @id_portifolio)
end

go
create or ALTER   procedure [dbo].[spListagemCarteiras]
(
   @id_usuario int
)
as
begin
 select c.* from Carteira c
 inner join Portifolio p on p.id_portifolio = c.id_portifolio
 inner join Usuario u on u.id_usuario = p.id_usuario where u.id_usuario = @id_usuario
end



------------------------------------------------SP's Movimentacao---------------------------------------------------------
go
create or alter procedure spConsulta_MovimentacaoPorCarteira
(
   @id_carteira int
)
as
begin
 select * from Movimentacao where id_carteira = @id_carteira
end
GO

GO
ALTER procedure [dbo].[spConsultaAvancadaMovimentacoes]
(
@carteira int,
@dataInicial datetime,
@dataFinal datetime)
as
begin
	declare @categIni int
	declare @categFim int
	set @categIni = case @carteira when 0 then 0 else @carteira end
	set @categFim = case @carteira when 0 then 999999 else @carteira end
	 select m.*, c.descricao as 'DescricaoCarteira', a.ticker as 'DescricaoTicker'
	from Movimentacao m
	inner join Carteira c on m.id_carteira = c.id_carteira
	inner join Ativo a on a.id_ativo = m.id_ativo
	where m.data_hora_movimentacao between @dataInicial and @dataFinal and
	m.id_carteira between @categIni and @categFim; 
end

go
create or ALTER procedure [dbo].[spListagemOperacao]
(
   @id_tabela_Geral int
)
as
begin
	select * from Item_Tabela_Geral where id_tabela_geral = @id_tabela_Geral
end

  go
create or alter procedure spInsert_Movimentacao
(
	@id int,
	@id_carteira int,
	@id_ativo int,
	@id_operacao int,
	@quantidade int,
	@preco decimal(10,2),
	@data_hora_movimentacao datetime
)
as
begin
	insert into Movimentacao (id_carteira, id_ativo, id_operacao, quantidade, preco, data_hora_movimentacao) 
			            values (@id_carteira, @id_ativo, @id_operacao, @quantidade, @preco, @data_hora_movimentacao) 
end