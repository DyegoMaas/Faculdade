cd C:\Users\OECKSLERS\Desktop\SD - Working LR1\SD - Corba - LogTransporte\src
set path=%path%;"C:\Program Files\Java\jdk1.7.0_07\bin"
idlj -fall LogisticaTransporte.idl
javac -cp "C:\Users\OECKSLERS\Desktop\SD - Working LR1\SD - Corba - LogTransporte\src" LogisticaTransporte\*.java
set classpath=.
javac *.java
start orbd
start java LogisticaTransporte.servidor