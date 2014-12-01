set path=%path%;"C:\Program Files\Java\jdk1.7.0_80\bin"

cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\RMI\bin
set classpath=.
rmic -keep arquivos.ServicoArquivosRemotosServidor

start java arquivos.ServicoArquivosRemotosServidor

