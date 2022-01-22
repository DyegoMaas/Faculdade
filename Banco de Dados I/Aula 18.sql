SELECT e.ename AS Nome, e.empno AS Numero, d.dname AS Departamento
FROM emp e, dept d
WHERE e.deptno = d.deptno;

SELECT d.dname AS Departamento, d.loc AS Localizacao, e.ename AS Nome, e.job AS Atividade 
FROM emp e, dept d
WHERE e.deptno = d.deptno AND d.loc LIKE 'NEW YORK';

SELECT e.ename AS Nome, d.dname AS Departamento, d.loc AS Localizacao
FROM emp e, dept d
WHERE e.deptno = d.deptno AND e.comm IS NOT NULL;

SELECT e.ename AS Nome, e.job AS Atividade, d.dname AS Departamento, e.sal AS Salario, s.grade AS Classificacao
FROM emp e, dept d, salgrade s
WHERE e.deptno = d.deptno AND e.sal BETWEEN s.losal AND s.hisal
GROUP BY ename, job, dname, sal, grade;

SELECT e.ename AS Nome_Empregado, e.empno AS Empregado, m.ename AS Nome_Gerente, m.empno AS Gerente
FROM emp e, emp m
WHERE e.mgr = m.empno;

SELECT e.ename AS Nome, d.dname AS Departamento, s.grade AS Classificacao
FROM emp e, dept d, salgrade s
WHERE e.deptno = d.deptno AND e.sal BETWEEN s.losal AND s.hisal
GROUP BY ename, dname, sal, grade;