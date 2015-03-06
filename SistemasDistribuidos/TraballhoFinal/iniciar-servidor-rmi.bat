call setar-path.bat

set classpath=C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\RMI\src
rmic -keep arquivos.ServicoArquivosRemotosServidor

cd C:\dev\Faculdade\SistemasDistribuidos\TraballhoFinal\RMI\bin
set classpath=.
rmic -keep arquivos.ServicoArquivosRemotosServidor

start java arquivos.ServicoArquivosRemotosServidor

