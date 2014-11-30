set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"

cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src\
idlj -fall ServicoAutenticacao.idl

set classpath=..\bin
javac -cp "C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src" autenticacao\*.java

start orbd -ORBInitialPort 2000
start java autenticacao.servidor -ORBInitialPort 2000 

