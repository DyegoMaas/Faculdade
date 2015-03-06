call setar-path.bat

cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src\
idlj -fall ServicoAutenticacao.idl

set classpath=.
javac -cp "C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src" autenticacao\*.java

start orbd -ORBInitialPort 2000
java autenticacao.servidor -ORBInitialPort 2000

