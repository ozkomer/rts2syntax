/**
 * 
 */
package rtsps;

import java.util.Vector;

/**
 * @author eduardo
 *
 */
public class Propuesta {

	public String name;
	public RaDec radec;
	FilterHeader filterH;

	public Propuesta (String request)
	{
		String block[];
		block = request.split(" ");
		int largo, cont;
		largo = block.length;
		String bloque;
		cont=0;
		String Ra[];
		String Decl[];
		Ra = new String[3];
		Decl = new String[3];


		for ( int i=0;i<largo;i++)
		{
			bloque = block[i].trim();
			if (bloque.length()>0)
			{
				//System.out.println("block["+cont+"]="+bloque);
				switch (cont) {
				case 0:
					this.name = bloque;
					break;
				case 1:
					Ra[0]=bloque;
					break;
				case 2:
					Ra[1]=bloque;
					break;
				case 3:
					Ra[2]=bloque;
					break;
				case 4:
					Decl[0]=bloque;
					break;
				case 5:
					Decl[1]=bloque;
					break;
				case 6:
					Decl[2]=bloque;
					break;
				case 7:
					filterH = new FilterHeader(bloque);
					break;
				default:
					if ( (bloque.indexOf('=')>0) && (bloque.indexOf('x')>0))
					{
						filterH.updateHeader(bloque);
						//System.out.println("FILTRO!!!\n");
					}
					break;
				}
				cont++;
			}
		}
		radec = new RaDec(Ra, Decl);
	}



	public String getName() {
		return name;
	}



	public void setName(String name) {
		this.name = name;
	}



	public RaDec getRadec() {
		return radec;
	}



	public void setRadec(RaDec radec) {
		this.radec = radec;
	}



	public FilterHeader getFilterH() {
		return filterH;
	}



	public void setFilterH(FilterHeader filterH) {
		this.filterH = filterH;
	}



	/**
	 * @param args
	 */
	public static void main(String[] args) {
		test2();
	}

	private static void test1 ()
	{
		Propuesta prop;
		String request;
		request = "2011hs         22 57 11.77 -43 23 04.8   BVu'g'r'i'z' B=15x60 V=11x60 u'=25x60 g'=r'=i'=11x60 z'=15x60";
		prop = new Propuesta(request);
		System.out.println("prop.name="+prop.getName());
		System.out.println("RA="+prop.getRadec().getRa());
		System.out.println("DEC="+prop.getRadec().getDec());
		System.out.println("Filters Header="+prop.getFilterH());
	}

	private static void test2() {
		Vector<String> request;
		Vector<Propuesta> vprop;
		request = new Vector<String>();
		vprop = new Vector<Propuesta>();
		/*
2011hs         22 57 11.77 -43 23 04.8   BVu'g'r'i'z' B=15x60, V=11x60 u'=25x60 g'=r'=i'=11x60 z'=15x60
2011iv  03 38 51.35 -35 35 32.0  BVu'g'r'i'Z  B=3x60 u=5x60 V=g'i'r'=3x60 Z=5x60
tphe0000       00:30:15.8 -46:30:02 BV B=3x30 V=3x30
2011ii         07 16 34.080 -38 29 03.81 BVg'r'i' B=25x60 V=g'=r'=i'=17x60
Rubin149       07 24 14.0 -00 31 38.0   BVu'g'r'i'z' B=3x30 V=3x30 u'=3x60 g'=r'=i'=3x30 z'=3x60
2011hp         12 16 25.47 -43 19 46.9   BVg'r'i' B=15x60, V=g'=r'=i'=11x60		
		 */
		//2011_12_07
		/*
		request.add("2011hs         22 57 11.77 -43 23 04.8   BVu'g'r'i'z' B=15x60 V=11x60 u'=25x60 g'=r'=i'=11x60 z'=15x60");
		request.add("2011iv  03 38 51.35 -35 35 32.0  BVu'g'r'i'Z  B=3x60 u=5x60 V=g'i'r'=3x60 Z=5x60");
		request.add("tphe0000       00 30 15.8 -46 30 02 BV B=3x30 V=3x30");
		request.add("2011ii         07 16 34.080 -38 29 03.81 BVg'r'i' B=25x60 V=g'=r'=i'=17x60");
		request.add("2011hp         12 16 25.47 -43 19 46.9   BVg'r'i' B=15x60 V=g'=r'=i'=11x60");
		 */
		//2011_12_08
		request.add("sn2011hs         22 57 11.77 -43 23 04.8   BVu'g'R'i'z' B=15x60 V=11x60 u'=25x60 g'=r'=i'=11x60 z'=15x60");
		request.add("sn2011iv  03 38 51.35 -35 35 32.0  BVu'g'r'i'Z  B=3x60 u=5x60 V=g'=i'=r'=3x60 Z=5x60");
		request.add("tphe0000       00 30 15.8 -46 30 02 BV B=3x30 V=3x30");
		request.add("Rubin149       07 24 14.0 -00 31 38.0   BVu'g'r'i'z' B=3x30 V=3x30 u'=3x60 g'=r'=i'=3x30 z'=3x60");
		request.add("sn2011ir 11 48 00.32 04 29 47.1   B=15x60 V=g'=r'=i'=11x60");

		for (int i=0;i<request.size();i++)
		{
			Propuesta prop;
			prop = new Propuesta(request.elementAt(i));
			vprop.add(prop);
		}
		for (int i=0;i<vprop.size();i++)
		{
			Propuesta prop;
			prop = vprop.elementAt(i);

			System.out.println("---------------------------------");
			System.out.println("prop.name="+prop.getName());
			System.out.println("RA="+prop.getRadec().getRa());
			System.out.println("DEC="+prop.getRadec().getDec());
			System.out.println("Filters Header="+prop.getFilterH());
		}

		System.out.println("---------------comandos rts2-newtarget------------------");
		for (int i=0;i<vprop.size();i++)
		{
			Propuesta prop;
			prop = vprop.elementAt(i);

			System.out.println(prop.comandoNewTarget());
		}
		System.out.println("---------------comandos rts2-target------------------");
		for (int i=0;i<vprop.size();i++)
		{
			Propuesta prop;
			prop = vprop.elementAt(i);
			System.out.println(prop.comandoTarget());
		}
		System.out.println("---------------comandos CalculoVisibilidad------------------");
		for (int i=0;i<vprop.size();i++)
		{
			Propuesta prop;
			prop = vprop.elementAt(i);
			System.out.println(prop.getCalculoVisibilidad());
		}
	}

	//rts2-newtarget --program chase_followup --database stars -ma PSN_J03385135-3535320 '03:38:51:35 -35:35:32:0'
	public String comandoNewTarget ()
	{
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append("rts2-newtarget --program chase_followup --database stars -ma ");
		sb.append(this.name);
		sb.append(" ");
		sb.append(this.radec);
		return sb.toString();
	}

	//rts2-target -c C0 -e -p 0 -s 'filter=CLR for 4 { E 20 } filter=B for 11 { E 60 } filter=V for 7 { E 60 } filter=u for 5 { E 60 } filter=g for 7 { E 60 } filter=r for 7 { E 60 } filter=i for 7 { E 60 } filter=Z for 5 { E 60 }' PSN_J03385135-3535320
	public String comandoTarget ()
	{
		StringBuilder sb;
		sb = new StringBuilder();
		sb.append("rts2-target -c C0 -e -p 0 -s  ");
		sb.append(this.filterH.getScript());
		sb.append(" ");
		sb.append(this.name);

		return sb.toString();
	}

	/**
	 * retorna algo del tipo:
	 * [2011hs] 22 57 11.77 -43 23 04.8
	 * @return
	 */
	public String getCalculoVisibilidad()
	{

		StringBuilder sb;
		sb = new StringBuilder();
		sb.append("[");
		sb.append(name);
		sb.append("] ");
		sb.append(this.radec.getFormatoVisibilidad());
		return sb.toString();
	}

}
