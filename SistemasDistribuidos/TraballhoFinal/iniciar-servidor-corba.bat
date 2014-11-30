cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src
set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"
idlj -fall corba\boasvindas\BoasVindas.idl
javac -cp "C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src" *\**\*.java
set classpath=.
javac *.java
start orbd -ORBInitialPort 2000
start java servidor.servidor -ORBInitialPort 2000 