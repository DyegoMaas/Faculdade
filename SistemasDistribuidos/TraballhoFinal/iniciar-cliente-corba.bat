set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"

cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src\
set classpath=..\bin
javac -cp "C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\CORBA\src" *.java

start java cliente -ORBInitialPort 2000 -ORBInitialHost localhost