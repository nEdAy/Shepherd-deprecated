using System;
namespace MvcApplication1.Model{
public class Message{

public string objectId  { get; set; }
public DateTime updatedAt  { get; set; }
public DateTime createdAt  { get; set; }
public string toldId  { get; set; }
public string msg  { get; set; }
public _User _User  { get; set; }
}
}