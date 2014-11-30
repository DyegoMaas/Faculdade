cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\RMI\bin
set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"
set classpath=.
rmic -keep rmi.helloworld.HelloServer
start rmiregistry
start java rmi.helloworld.HelloServer
