create table PESSOA(
	ID int8 not null,
	NOME varchar(60) not null,
	CPF int8 not null,
	Unique(CPF),
	primary key (ID)
);
create sequence SEQ_PESSOA;
alter table PESSOA alter column ID set default nextval('"seq_pessoa"');