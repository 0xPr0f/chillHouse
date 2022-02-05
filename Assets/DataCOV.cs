using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------- COLLECTION DATA ------------//
public class ExternalData
{
    public string name { get; set; }
    public string description { get; set; }
    public string image { get; set; }
    public string image_256 { get; set; }
    public string image_512 { get; set; }
    public string image_1024 { get; set; }
    public object animation_url { get; set; }
    public object external_url { get; set; }
    public object attributes { get; set; }
    public object owner { get; set; }
}
[Serializable]
public class NftData
{
    public string token_id { get; set; }
    public string token_balance { get; set; }
    public string token_url { get; set; }
    public List<string> supports_erc { get; set; }
    public object token_price_wei { get; set; }
    public object token_quote_rate_eth { get; set; }
    public string original_owner { get; set; }
    public ExternalData external_data { get; set; }
    public string owner { get; set; }
    public string owner_address { get; set; }
    public bool burned { get; set; }
}
[Serializable]
public class Item
{
    public int contract_decimals { get; set; }
    public string contract_name { get; set; }
    public string contract_ticker_symbol { get; set; }
    public string contract_address { get; set; }
    public List<string> supports_erc { get; set; }
    public string logo_url { get; set; }
    public object last_transferred_at { get; set; }
    public string type { get; set; }
    public object balance { get; set; }
    public object balance_24h { get; set; }
    public object quote_rate { get; set; }
    public object quote_rate_24h { get; set; }
    public object quote { get; set; }
    public object quote_24h { get; set; }
    public List<NftData> nft_data { get; set; }
}
[Serializable]
public class Data
{
    public string updated_at { get; set; }
    public List<Item> items { get; set; }
    public object pagination { get; set; }
}

public class Root
{
    public Data data { get; set; }
    public bool error { get; set; }
    public object error_message { get; set; }
    public object error_code { get; set; }
}

//------- COLLECTION DATA ------------//

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Itemcov
{
    public int contract_decimals { get; set; }
    public string contract_name { get; set; }
    public string contract_ticker_symbol { get; set; }
    public string contract_address { get; set; }
    public object supports_erc { get; set; }
    public string logo_url { get; set; }
    public string token_id { get; set; }
}

public class Pagination
{
    public bool has_more { get; set; }
    public int page_number { get; set; }
    public int page_size { get; set; } 
    public int total_count { get; set; }
}

public class Datacov
{
    public string updated_at { get; set; }
    public List<Itemcov> items { get; set; }
    public Pagination pagination { get; set; }
}

public class Rootcov
{
    public Datacov data { get; set; }
    public bool error { get; set; }
    public object error_message { get; set; }
    public object error_code { get; set; }
}

