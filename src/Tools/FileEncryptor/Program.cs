using GTMH.Security;
using GTMH.Util;

Func<int> PrintUsage=()=>
{
  Console.WriteLine("Usage:");
  Console.WriteLine("Optional: --secret=<secret>");
  Console.WriteLine("Required: --input=<a_file>");
  Console.WriteLine("Required: --output=<a_file>");
  Console.WriteLine("Required: --action=<action>");
  Console.WriteLine("where <action> must be encrypt or decrypt");
  return -1;
};

var secret = args.GetCmdLine("secret");

var input = args.GetCmdLine("input");
if(input == null || !System.IO.File.Exists(input)) return PrintUsage();

var output = args.GetCmdLine("output");
if ( output==null ) return PrintUsage();


if(secret == null)
{
  Console.WriteLine("Enter secret");
  secret=Console.ReadLine();
  if ( secret == null) return PrintUsage();
}

try
{
  var input_data = System.IO.File.ReadAllBytes(input);
  byte[] output_data;
  var encryptor = new CipherEncryption(secret);
  switch(args.GetCmdLine("action", "unspecified").ToLower())
  {
    case "encrypt":
    {
      output_data = encryptor.Encrypt(input_data);
      break;
    }
    case "decrypt":
    {
      output_data = encryptor.Decrypt(input_data);
      break;
    }
    default:
    {
      return PrintUsage();
    }
  }
  System.IO.File.WriteAllBytes(output, output_data);
}
catch(Exception e)
{
  Console.WriteLine($"Error in main: {e}");
  return -2;
}



return 0;
