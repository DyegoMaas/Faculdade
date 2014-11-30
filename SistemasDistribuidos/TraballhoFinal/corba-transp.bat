cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA
set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"
idlj -fall BoasVindas.idl
javac -cp "C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA" BoasVindas\*.java
set classpath=.
javac *.java
start orbd
start java servidor.servidor