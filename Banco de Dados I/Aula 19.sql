SELECT MIN(sal) Minimo, AVG(sal) Media, MAX(sal) Maximo, SUM(sal) Soma
FROM emp;

SELECT job Cargo, MIN(sal) Minimo, MAX(sal) Maximo, AVG(sal) Media, SUM(sal) Soma
FROM emp
GROUP BY job;

SELECT job, COUNT(*) NumeroEmpregados
FROM emp
GROUP BY job
HAVING COUNT(*) > 0;

SELECT COUNT(*) NumeroGerentes
FROM emp
WHERE job LIKE 'MANAGER';

SELECT dept.dname AS  Departamento, dept.loc AS LocalDepartamento, COUNT(emp.empno) AS NumeroFuncionarios, AVG(emp.sal) AS MediaSalarios
FROM emp
LEFT JOIN dept ON emp.deptno = dept.deptno
GROUP BY dname;

SELECT dept.dname AS Departamento
FROM emp
LEFT JOIN dept ON emp.deptno = dept.deptno
GROUP BY dname
HAVING AVG(sal) > 2000;

SELECT dept.dname AS  Departamento, dept.loc AS LocalDepartamento
FROM emp
LEFT JOIN dept ON emp.deptno = dept.deptno
GROUP BY dname, loc
HAVING COUNT(emp.deptno) = 5;