package autenticacao;


/**
* autenticacao/Servico_AutenticacaoHelper.java .
* Generated by the IDL-to-Java compiler (portable), version "3.2"
* from ServicoAutenticacao.idl
* Sunday, November 30, 2014 3:42:31 PM BRST
*/

abstract public class Servico_AutenticacaoHelper
{
  private static String  _id = "IDL:autenticacao/Servico_Autenticacao:1.0";

  public static void insert (org.omg.CORBA.Any a, autenticacao.Servico_Autenticacao that)
  {
    org.omg.CORBA.portable.OutputStream out = a.create_output_stream ();
    a.type (type ());
    write (out, that);
    a.read_value (out.create_input_stream (), type ());
  }

  public static autenticacao.Servico_Autenticacao extract (org.omg.CORBA.Any a)
  {
    return read (a.create_input_stream ());
  }

  private static org.omg.CORBA.TypeCode __typeCode = null;
  synchronized public static org.omg.CORBA.TypeCode type ()
  {
    if (__typeCode == null)
    {
      __typeCode = org.omg.CORBA.ORB.init ().create_interface_tc (autenticacao.Servico_AutenticacaoHelper.id (), "Servico_Autenticacao");
    }
    return __typeCode;
  }

  public static String id ()
  {
    return _id;
  }

  public static autenticacao.Servico_Autenticacao read (org.omg.CORBA.portable.InputStream istream)
  {
    return narrow (istream.read_Object (_Servico_AutenticacaoStub.class));
  }

  public static void write (org.omg.CORBA.portable.OutputStream ostream, autenticacao.Servico_Autenticacao value)
  {
    ostream.write_Object ((org.omg.CORBA.Object) value);
  }

  public static autenticacao.Servico_Autenticacao narrow (org.omg.CORBA.Object obj)
  {
    if (obj == null)
      return null;
    else if (obj instanceof autenticacao.Servico_Autenticacao)
      return (autenticacao.Servico_Autenticacao)obj;
    else if (!obj._is_a (id ()))
      throw new org.omg.CORBA.BAD_PARAM ();
    else
    {
      org.omg.CORBA.portable.Delegate delegate = ((org.omg.CORBA.portable.ObjectImpl)obj)._get_delegate ();
      autenticacao._Servico_AutenticacaoStub stub = new autenticacao._Servico_AutenticacaoStub ();
      stub._set_delegate(delegate);
      return stub;
    }
  }

  public static autenticacao.Servico_Autenticacao unchecked_narrow (org.omg.CORBA.Object obj)
  {
    if (obj == null)
      return null;
    else if (obj instanceof autenticacao.Servico_Autenticacao)
      return (autenticacao.Servico_Autenticacao)obj;
    else
    {
      org.omg.CORBA.portable.Delegate delegate = ((org.omg.CORBA.portable.ObjectImpl)obj)._get_delegate ();
      autenticacao._Servico_AutenticacaoStub stub = new autenticacao._Servico_AutenticacaoStub ();
      stub._set_delegate(delegate);
      return stub;
    }
  }

}
