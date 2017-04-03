using System;
namespace MvcApplication1.Model{
public class Comment{

public string objectId  { get; set; }
public string parentId  { get; set; }
public string contents  { get; set; }
public int replyCount  { get; set; }
public DateTime updatedAt  { get; set; }
public DateTime createdAt  { get; set; }
public _User _User  { get; set; }
}
}