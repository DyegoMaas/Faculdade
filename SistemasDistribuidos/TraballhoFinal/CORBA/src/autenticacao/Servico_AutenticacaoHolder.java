package autenticacao;

/**
* autenticacao/Servico_AutenticacaoHolder.java .
* Generated by the IDL-to-Java compiler (portable), version "3.2"
* from ServicoAutenticacao.idl
* Sunday, November 30, 2014 3:42:31 PM BRST
*/

public final class Servico_AutenticacaoHolder implements org.omg.CORBA.portable.Streamable
{
  public autenticacao.Servico_Autenticacao value = null;

  public Servico_AutenticacaoHolder ()
  {
  }

  public Servico_AutenticacaoHolder (autenticacao.Servico_Autenticacao initialValue)
  {
    value = initialValue;
  }

  public void _read (org.omg.CORBA.portable.InputStream i)
  {
    value = autenticacao.Servico_AutenticacaoHelper.read (i);
  }

  public void _write (org.omg.CORBA.portable.OutputStream o)
  {
    autenticacao.Servico_AutenticacaoHelper.write (o, value);
  }

  public org.omg.CORBA.TypeCode _type ()
  {
    return autenticacao.Servico_AutenticacaoHelper.type ();
  }

}
