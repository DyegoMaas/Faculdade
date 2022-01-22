/*Questão 1*/
create table Medicamento (
    cd_medicamento integer NOT NULL AUTO_INCREMENT,
    nm_medicamento varchar(50) NOT NULL,
    cd_anvisa varchar(20) NOT NULL,
    ds_medicamento varchar(200),
    vl_venda float(7,2) NOT NULL,
    vl_custo float(7,2) NOT NULL,
    qt_estoque int default 0,
    primary key (cd_medicamento),
    CONSTRAINT anvisa_UK UNIQUE (cd_anvisa)
);

create table Fornecedor (
    cd_fornecedor integer NOT NULL AUTO_INCREMENT,
    nm_fornecedor varchar(50) NOT NULL,
    nr_telefone varchar(15),
    ds_website varchar(100),
    primary key (cd_fornecedor)
);

create table Fornecedor_Medicamento (
    cd_medicamento integer NOT NULL,
    cd_fornecedor integer NOT NULL,
    foreign key (cd_medicamento) references Medicamento(cd_medicamento),
    foreign key (cd_fornecedor) references Fornecedor(cd_fornecedor),
    CONSTRAINT med_for_UK UNIQUE (cd_medicamento, cd_fornecedor)
);

create table Categoria (
    cd_categoria integer NOT NULL AUTO_INCREMENT,
    ds_categoria varchar(50) NOT NULL,
    primary key (cd_categoria)
);

create table Categoria_Medicamento (
    cd_medicamento integer NOT NULL,
    cd_categoria integer NOT NULL,
    foreign key (cd_medicamento) references Medicamento(cd_medicamento),
    foreign key (cd_categoria) references Categoria(cd_categoria),
    CONSTRAINT med_cat_UK UNIQUE (cd_medicamento, cd_categoria)
);

/*Questão 2*/
insert into Fornecedor (nm_fornecedor, nr_telefone, ds_website)
values('fornecedor 1', '47 3399 3333', 'www.site.com');

insert into Fornecedor (nm_fornecedor, nr_telefone, ds_website)
values('fornecedor 2', '(47) 9911-3333', 'www.site2.com');

insert into Fornecedor (nm_fornecedor, nr_telefone, ds_website)
values('fornecedor 2', '(49)99412223', NULL);

insert into Medicamento (nm_medicamento, cd_anvisa, ds_medicamento, vl_venda, vl_custo, qt_estoque)
values ('medicamento 1', 'RV123123456', 'descrição do medicamento 1', 50.2, 35, 10);

insert into Medicamento (nm_medicamento, cd_anvisa, ds_medicamento, vl_venda, vl_custo, qt_estoque)
values ('medicamento 2', 'RV12312388', 'descrição do medicamento 2', 50.2, 35, 10);

insert into Medicamento (nm_medicamento, cd_anvisa, ds_medicamento, vl_venda, vl_custo, qt_estoque)
values ('medicamento 3', 'RV312388', NULL, 20, 25, 20);

insert into Medicamento (nm_medicamento, cd_anvisa, ds_medicamento, vl_venda, vl_custo, qt_estoque)
values ('medicamento 4', 'RV36612388', 'descrição do medicamento 4', 100, 25, 200);

insert into Medicamento (nm_medicamento, cd_anvisa, ds_medicamento, vl_venda, vl_custo, qt_estoque)
values ('medicamento 5', 'RV36655588', 'descrição do medicamento 5', 81.9, 70, 35);

insert into Fornecedor_Medicamento (cd_fornecedor, cd_medicamento)
values (1, 2);

insert into Fornecedor_Medicamento (cd_fornecedor, cd_medicamento)
values (2, 2);

insert into Fornecedor_Medicamento (cd_fornecedor, cd_medicamento)
values (3, 1);

insert into Fornecedor_Medicamento (cd_fornecedor, cd_medicamento)
values (3, 3);

insert into Fornecedor_Medicamento (cd_fornecedor, cd_medicamento)
values (2, 3);

insert into Fornecedor_Medicamento (cd_fornecedor, cd_medicamento)
values (2, 4);

insert into Fornecedor_Medicamento (cd_fornecedor, cd_medicamento)
values (3, 5);

insert into Categoria (ds_categoria) values ('categoria 1');
insert into Categoria (ds_categoria) values ('categoria 2');
insert into Categoria (ds_categoria) values ('categoria 3');

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (1, 1);

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (1, 2);

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (2, 2);

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (3, 1);

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (3, 2);

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (3, 3);

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (5, 1);

insert into Categoria_Medicamento (cd_medicamento, cd_categoria)
values (5, 3);

/*Questão 3*/    
select m.nm_medicamento, m.vl_custo, m.qt_estoque, c.ds_categoria
from Medicamento m 
left join Categoria_Medicamento cm on cm.cd_medicamento = m.cd_medicamento
left join Categoria c on cm.cd_categoria = c.cd_categoria;
    
/*Questão 4*/    
select c.ds_categoria as Categoria, COUNT(cm.cd_medicamento) as Medicamentos
from Categoria c, Categoria_Medicamento cm
where cm.cd_categoria = c.cd_categoria
group by ds_categoria
order by COUNT(cm.cd_medicamento) desc;

/*Questão 5*/
create view medicamentos_por_fornecedor as
    select m.nm_medicamento, f.nm_fornecedor
    from Medicamento m, Fornecedor f, Fornecedor_Medicamento fm
    where 
        fm.cd_medicamento = m.cd_medicamento and
        fm.cd_fornecedor = f.cd_fornecedor and
        m.cd_medicamento in (select cd_medicamento from Fornecedor_Medicamento
                                group by cd_medicamento
                                having count(cd_fornecedor) > 1)
    group by nm_medicamento, nm_fornecedor;