using Newtonsoft.Json;
using System;
namespace MvcApplication1.Model{
public class FavoriteItem{

public long uuid  { get; set; }
public long num_iid  { get; set; }
public long favorites_id  { get; set; }
public string title  { get; set; }
public string pict_url  { get; set; }
public string small_images  { get; set; }
public string reserve_price  { get; set; }
public string zk_final_price  { get; set; }
public long user_type  { get; set; }
public string provcity  { get; set; }
public string item_url  { get; set; }
public string click_url  { get; set; }
public long volume  { get; set; }
public string tk_rate  { get; set; }
public string zk_final_price_wap  { get; set; }
public DateTime event_start_time  { get; set; }
public DateTime event_end_time  { get; set; }
}
}