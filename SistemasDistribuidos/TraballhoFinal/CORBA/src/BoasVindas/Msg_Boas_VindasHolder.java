package BoasVindas;

/**
* BoasVindas/Msg_Boas_VindasHolder.java .
* Generated by the IDL-to-Java compiler (portable), version "3.2"
* from BoasVindas.idl
* Sunday, November 30, 2014 2:48:21 PM BRST
*/

public final class Msg_Boas_VindasHolder implements org.omg.CORBA.portable.Streamable
{
  public BoasVindas.Msg_Boas_Vindas value = null;

  public Msg_Boas_VindasHolder ()
  {
  }

  public Msg_Boas_VindasHolder (BoasVindas.Msg_Boas_Vindas initialValue)
  {
    value = initialValue;
  }

  public void _read (org.omg.CORBA.portable.InputStream i)
  {
    value = BoasVindas.Msg_Boas_VindasHelper.read (i);
  }

  public void _write (org.omg.CORBA.portable.OutputStream o)
  {
    BoasVindas.Msg_Boas_VindasHelper.write (o, value);
  }

  public org.omg.CORBA.TypeCode _type ()
  {
    return BoasVindas.Msg_Boas_VindasHelper.type ();
  }

}
