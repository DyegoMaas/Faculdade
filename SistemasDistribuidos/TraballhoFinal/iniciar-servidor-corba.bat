set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"

cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src\
idlj -fall BoasVindas.idl

set classpath=..\bin
javac -cp "C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src" BoasVindas\*.java
javac *.java

start orbd -ORBInitialPort 2000
start java BoasVindas.servidor -ORBInitialPort 2000 