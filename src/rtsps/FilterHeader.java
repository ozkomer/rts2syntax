package rtsps;

import java.util.Hashtable;
import java.util.Vector;

public class FilterHeader {

	public Vector<Filter> header;

	public static Hashtable<String, String> dictFiltro = null;



	public FilterHeader(String encabezado)
	{
		if (dictFiltro==null){
			dictFiltro = new Hashtable<String, String>();
			dictFiltro.put("B", "BBB");
			dictFiltro.put("V", "VVV");
			dictFiltro.put("u", "uuu");
			dictFiltro.put("g", "ggg");
			dictFiltro.put("r", "rrr");
			dictFiltro.put("i", "iii");
			dictFiltro.put("Z", "ZZZ");
		}
		header = new Vector<Filter>();
		int largoEncabezado;
		largoEncabezado = encabezado.length();
		char letraFiltro;
		for (int i=0;i<largoEncabezado;i++)
		{
			letraFiltro = encabezado.toLowerCase().charAt(i);
			switch (letraFiltro) {
			case 'b':
				header.add(new Filter("B", -1, -1));
				break;
			case 'v':
				header.add(new Filter("V", -1, -1));
				break;
			case 'u':
				header.add(new Filter("u", -1, -1));
				break;
			case 'g':
				header.add(new Filter("g", -1, -1));
				break;
			case 'r':
				header.add(new Filter("r", -1, -1));
				break;
			case 'i':
				header.add(new Filter("i", -1, -1));
				break;
			case 'z':
				header.add(new Filter("Z", -1, -1));
				break;
			default:
				break;
			}
		}
	}

	public void updateHeader (String instructions)
	{
		String block[];
		block = instructions.split("=");
		int largo;
		largo = block.length;
		String repXexp;
		repXexp = block[largo-1];
		String[] repex;
		repex = repXexp.split("x");
		int repet, expos;
		repet = Integer.parseInt(repex[0]);
		expos = Integer.parseInt(repex[1]);
		int largoHeader;
		largoHeader = header.size();
		Filter filtroLocal;

		String first;
		for (int i=0;i<largo-1;i++)
		{
			first = block[i].substring(0,1);
			//System.out.println("block["+i+"]="+first);
			for (int j=0;j<largoHeader;j++)
			{
				filtroLocal = header.elementAt(j);
				if (first.toLowerCase().equals(filtroLocal.getCode().toLowerCase()))
				{
					filtroLocal.setRepetitions(repet);
					filtroLocal.setExposure(expos);
				}
			}

		}


	}

	@Override
	public String toString() {
		StringBuilder sb;
		sb = new StringBuilder();
		for ( int i=0;i<header.size();i++)
		{
			sb.append(header.elementAt(i));
			sb.append(" | ");
		}
		return sb.toString();
	}

	/**
	 * Utilizado por el comanto rts2-target
	 * Debe entregar algo como...
	 * 'filter=CLR for 4 { E 20 } filter=B for 11 { E 60 } filter=V for 7 { E 60 } filter=u for 5 { E 60 } filter=g for 7 { E 60 } filter=r for 7 { E 60 } filter=i for 7 { E 60 } filter=Z for 5 { E 60 }'
	 * @return
	 */
	public String getScript() {
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append("'");
		int headerSize;
		headerSize = header.size();
		for ( int i=0;i<headerSize;i++)
		{
			sb.append(header.elementAt(i).getScript());
			if (i<(headerSize-1)) 
				sb.append(" ");
		}
		sb.append("'");
		return sb.toString();
	}


}
