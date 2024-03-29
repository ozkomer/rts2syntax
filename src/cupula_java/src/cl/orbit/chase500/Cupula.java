/**
 * 
 */
package cl.orbit.chase500;

import java.net.InetAddress;

import net.wimpi.modbus.Modbus;
import net.wimpi.modbus.ModbusException;
import net.wimpi.modbus.ModbusIOException;
import net.wimpi.modbus.ModbusSlaveException;
import net.wimpi.modbus.io.ModbusTCPTransaction;
import net.wimpi.modbus.msg.ReadInputDiscretesRequest;
import net.wimpi.modbus.msg.ReadInputDiscretesResponse;
import net.wimpi.modbus.net.TCPMasterConnection;

/**
 * @author sysop
 *
 */
public class Cupula {
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		/* The important instances of the classes mentioned before */
		TCPMasterConnection con = null; //the connection
		ModbusTCPTransaction trans = null; //the transaction
		ReadInputDiscretesRequest req = null; //the request
		ReadInputDiscretesResponse res = null; //the response

		/* Variables for storing the parameters */
		InetAddress addr = null; //the slave's address
		int port = Modbus.DEFAULT_PORT;
		int ref = 0; //the reference; offset where to start reading from
		int count = 0; //the number of DI's to read
		int repeat = 1; //a loop for repeating the transaction
		
		//1. Setup the parameters
		if (args.length < 3) {
			System.out.println("Cantidad insuficiente de parametros.");
		  System.exit(1);
		} else {
			System.out.println("args[0]="+args[0]);
			System.out.println("args[1]="+args[1]);
			System.out.println("args[2]="+args[2]);
		  try {
		    String astr = args[0];
		    int idx = astr.indexOf(':');
		    if(idx < 0) {
		      port = 502;
		      //astr = astr.substring(0,idx);
		    }
		    addr = InetAddress.getByName(astr);
		    ref = Integer.decode(args[1]).intValue();
		    count = Integer.decode(args[2]).intValue();
		    if (args.length == 4) {
		      repeat = Integer.parseInt(args[3]);
		    }
		  } catch (Exception ex) {
		    ex.printStackTrace();
		    System.exit(1);
		  }
		}
		
		//2. Open the connection
		con = new TCPMasterConnection(addr);
		con.setPort(port);
		try {
			con.connect();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		//3. Prepare the request
		req = new ReadInputDiscretesRequest(ref, count);

		//4. Prepare the transaction
		trans = new ModbusTCPTransaction(con);
		trans.setRequest(req);

		//5. Execute the transaction repeat times
		int k = 0;
		do {
		  try {
			trans.execute();
		} catch (ModbusIOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (ModbusSlaveException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (ModbusException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		  res =  (ReadInputDiscretesResponse) trans.getResponse();
		  System.out.println("Digital Inputs Status=" + res.getDiscretes().toString());
		  k++;
		} while (k < repeat);

		 //6. Close the connection
		 con.close();
	}

}
