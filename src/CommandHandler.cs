﻿namespace codecrafters_redis;

public class CommandHandler
{
    private String _response;
    public CommandHandler(String[] command, Store store)
    {
        String cmd = command[0];
        DateTime currTime = DateTime.Now;
        switch (cmd){
            case "ping":
                _response = "+PONG\r\n";
                break;
            case "echo":
                _response = $"+{command[1]}\r\n";
                break;
            case "get":
                try
                {
                    Get(command, store, currTime);
                }
                catch (KeyNotFoundException)
                {
                    _response = $"$-1\r\n";
                }
                break;
            case "set":

                Console.WriteLine("Current Time: " + currTime.ToString());
                Console.WriteLine("Command Length: "+command.Length);

                if (command.Length == 3)
                {
                    DateTime expiry = DateTime.MaxValue;
                    Value val = new Value(command[2], currTime, expiry);
                    store.GetMap()[command[1]] = val;
                }
                else if (command.Length == 5 && command[3].Equals("px"))
                {
                    int delta = int.Parse(command[4]);
                    
                    DateTime expiry = currTime.AddMilliseconds(delta);
                    Value val = new Value(command[2], currTime, expiry);
                    store.GetMap()[command[1]] = val;
                }







                Console.WriteLine("Keys: ");
                Dictionary<string, Value> g = store.GetMap();
                foreach (string key in g.Keys)
                {
                    Console.WriteLine(key + ": " + g[key].val);
                }
                _response = "+OK\r\n";
                break;
            default:
                _response = "+No Response\r\n";
                break;
        }
    }
    
    
    
    
    public String GetResponse()
    {
        return _response;
    }




    public void Get(String[] command,Store store,DateTime currTime)
    {
        Value val = store.GetMap()[command[1]];

        Console.WriteLine("Value: " + val.val);
        Console.WriteLine("Expiry: " + val.expiry.ToString());
        if (currTime <= val.expiry)
        {
            _response = $"+{val.val}\r\n";
        }
        else
        {
            store.GetMap().Remove(command[1]);
            _response = $"$-1\r\n";
        }
    }
}