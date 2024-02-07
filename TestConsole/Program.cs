using System.IdentityModel.Tokens.Jwt;

// A jwt encoded token string in this case extracted from the 'Authorization' HTTP header
var tokenString = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiKzk5ODk5OTEzMzgyMiIsImV4cCI6MTcwNzAyNzQxMH0.-SN_4KNlbgRKR7_XfiltuKwQzPrhVtxSD9iYagYLXg9Q6IVWMOzHHE93S4gRxbvqZHFs60Efm5QPFqNnHQ_18Q";

// Trim 'Bearer ' from the start since its just a prefix for the token
var jwtEncodedString = tokenString.Substring(7);

// Instantiate a new Jwt Security Token from the Jwt Encoded String
var token = new JwtSecurityToken(jwtEncodedString);
var json = token.Payload.Values;
Console.WriteLine(json);

// Retrieve info from the Json Web Token 
Console.WriteLine("phone number => " + token.Payload.FirstOrDefault().Value);
