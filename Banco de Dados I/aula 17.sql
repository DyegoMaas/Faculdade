SELECT * FROM dept WHERE dname LIKE 'SALES';

SELECT * FROM emp WHERE job LIKE 'ANALYST';

SELECT ename, sal, job 	FROM emp 
	WHERE comm IS NOT NULL 
	ORDER BY sal DESC;
	
SELECT ename, job, hiredate FROM emp
	WHERE deptno = 20 AND
	      sal > 1000;
	      
SELECT * FROM emp 
	WHERE job LIKE 'MANAGER';
	
SELECT * FROM emp 
	WHERE ename LIKE '%A%';

SELECT ename, deptno FROM emp 
	WHERE deptno = 10 OR
	      deptno = 30
	ORDER BY ename;
	
SELECT * FROM emp 
	WHERE hiredate BETWEEN '1982-01-01' AND '1982-12-31';	      
