set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"

cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\RMI\src
rmic -keep rmi.arquivos.ServicoArquivosRemotosServidor

set classpath=..\bin
javac -cp . rmi\arquivos\*.java
javac -cp . rmi\arquivos\modelos\*.java

java rmi.arquivos.ServicoArquivosRemotosServidor