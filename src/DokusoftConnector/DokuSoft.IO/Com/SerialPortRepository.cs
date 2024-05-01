using System.IO.Ports;
namespace Dokusoft.IO.Com;

public static class SerialPortRepository
{
	public static List<string> GetPorts()
	{
		var ports = SerialPort.GetPortNames();
		return ports.ToList();
	}
}