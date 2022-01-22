SELECT e.ename AS Funcionario, d.dname AS Departamento, e.job AS Funcao
FROM emp e, dept d
WHERE e.deptno = d.deptno AND 
      d.LOC LIKE 'DALLAS';
      
SELECT empno AS Numero, ename AS Funcionario
FROM emp
WHERE sal < (SELECT AVG(sal) FROM emp)
ORDER BY sal DESC;
      
SELECT empno AS Numero, ename AS Funcionario
FROM emp
WHERE mgr = (SELECT empno FROM emp WHERE ename LIKE 'King');

SELECT ename AS Funcionario
FROM emp
WHERE sal = (SELECT MAX(sal) FROM emp);

SELECT e.ename AS Funcionario, d.dname AS Departamento, d.loc AS Localizacao
FROM emp e, dept d
WHERE e.deptno = d.deptno AND 
      e.sal > (SELECT AVG(sal) FROM emp);
		     
SELECT dname AS Departamento
FROM dept
WHERE deptno = (SELECT d.deptno
		FROM dept d, emp e
		WHERE e.deptno = d.deptno 
		GROUP BY d.deptno
		ORDER BY COUNT(e.empno) DESC
		LIMIT 1);