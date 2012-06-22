package cl.calan.ctio.vr;


import java.awt.BorderLayout;
import java.awt.Canvas;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import javax.swing.JCheckBox;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JSlider;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;

import org.lwjgl.util.vector.Vector2f;

import com.threed.jpct.FrameBuffer;
import com.threed.jpct.IRenderer;
import com.threed.jpct.Object3D;
import com.threed.jpct.Primitives;
import com.threed.jpct.SimpleVector;
import com.threed.jpct.Texture;
import com.threed.jpct.TextureManager;
import com.threed.jpct.World;

/**
 * A simple HelloWorld using the AWTGL-Renderer and rendering into a frame.
 * 
 * @author EgonOlsen
 * 
 */
public class Gem3D implements MouseListener, MouseMotionListener,
		ChangeListener, ActionListener {

	public final static String UdpServerHost = "190.98.214.226";
	public final static int UdpServerPort = 19000;
	private World world;
	private FrameBuffer buffer;
	private Object3D observatorio;
	private Object3D ascencionRecta;
	private Object3D telescopio;

	private DatagramSocket socket;
	private javax.swing.Timer socketTimer;
	/**
	 * Al Activarlo, se activa el socket timer, al desactivarlo, se desactiva
	 * el socketTimer.
	 */
	private JCheckBox jcbChase500;
	
	/**
	 * El tubo del telescopio no está centrado respecto al eje de ascension recta.
	 * Aquí se define el desplazamiento respecto a dicho eje.
	 */
	public static final float TELESCOPE_SHIFT = -5.0f;

	private JFrame frame;
	
	/**
	 * Para explorar el observatorio con el mouse
	 */
	private int prevMouseX, prevMouseY;
	
	/**
	 * Vector Unitario de Declinacion. 
	 * Aqui solo se deben representar valores acordes con lo que se despliega en pantalla.
	 * Si al cambiar el angulo zenital aparecen posibles valores diferentes de declinacion,
	 * no debe usarse este campo para evaluar las alternativas.
	 */
	private SimpleVector decV;

	
	/**
	 * Para mover el eje de ascencion recta manualmente. En un telescopio
	 * convencional, este angulo corresponde a la ascención recta, el cual está en función
	 * de demasiadas variables, entre ellas, la hora sideral. 
	 * Como aquí solo pretendemos usar geometría euclidiana para nuestro modelo,
	 * definimos el "angulo del contrapeso", cwAngle:
	 * cwAngle = 0   <---> Contrapeso apuntando hacia arriba.
	 * cwAngle = 90  <---> Contrapeso apuntando hacia el West.
	 * cwAngle = 180 <---> Contrapeso apuntando hacia abajo.
	 * cwAngle = 270 <---> Contrapeso apuntando hacia el East.
	 */
	private JSlider sliderCWa;
	/**
	 * Angulo del contrapeso en radianes
	 */
	private float cwA;
	private float cwAngleLast;

	/**
	 * Para mover el eje de declinación, aquí se utilizan los mismos valores y unidades
	 * que en una montura Germano-Equatorial.
	 */
	private JSlider sliderDEC;
	/**
	 * Angulo de declinacion en radianes
	 */
	private float declination;
	private float lastangleDEC;

	private JSlider sliderZenith;
	
	/**
	 * Vector Apuntando al Zenith
	 */
	public final static SimpleVector ZENITH = new SimpleVector (0.0f,0.0f,1.0f);
	
	/**
	 * true->Movimientos del slider Zenith, son procesados.
	 * Util para hacer un update del angulo zenital al mover el telescopio.
	 */
	private boolean processZenith;
	
	/**
	 * true->Movimientos del slider DEC, son procesados.
	 * Util para hacer un update de la declinacion al mover el slider del angulo zenital.
	 */
	private boolean processDEC;

	/**
	 * Las rotaciones en declinación, dado el shift del telescopio, conllevan ademas
	 * translaciones del telescopio. (Estamos engañando al motor 3D, pues la rotaciones
	 * del tubo del telescopio las hacemos en el cntro del cilindro, pero luego las trasladamos
	 * al punto de apoyo del telescopio con el eje de ascencion recta.
	 */
	private float shiftX, lastShiftX;
	private float shiftZ, lastShiftZ;

	/**
	 * Cantidad de caras a presentar en los cilindros.
	 */
	public static final int FACES = 20;
	
	/**
	 * Constante para convertir grados sexagesimales, en radianes.
	 */
	public static final float TO_RADIANS = (float) (Math.PI / 180.0f);
	public static final float TO_GRAD = (float) (180.0f / Math.PI);
	
	public static final float PI_HALF = (float) (Math.PI / 2);

	/**
	 * Latitud del observatorio.
	 */
	public static final float LAT_OBSERVATORY = (float) (33.0 * TO_RADIANS);
	
	/**
	 * Angulo complementario de la Latitud del Observatorio
	 */
	public static final float LAT_OBSERVATORY_COMP = (PI_HALF - LAT_OBSERVATORY);
	

	public static void main(String[] args) throws Exception {
		new Gem3D().loop();
	}

	public Gem3D() throws Exception {

		frame = new JFrame("Chase 500 3D");
		frame.setSize(800, 700);
		frame.setVisible(true);
		frame.setDefaultCloseOperation(JFrame.HIDE_ON_CLOSE);
		Dimension sliderDimension;
		sliderDimension = new Dimension(30, 450);
		
		this.cwA = (float) Math.PI;
		sliderCWa = new JSlider(JSlider.VERTICAL, 0, 359, (int) (this.cwA * TO_GRAD));
		sliderCWa.setMajorTickSpacing(90);
		sliderCWa.setMajorTickSpacing(10);
		sliderCWa.setPaintTicks(true);
		sliderCWa.setPreferredSize(sliderDimension);
		sliderCWa.addChangeListener(this);
		
		this.declination = 0;
		sliderDEC = new JSlider(JSlider.VERTICAL, -180, 180, (int) (this.declination * TO_GRAD));
		sliderDEC.setMajorTickSpacing(90);
		sliderDEC.setMajorTickSpacing(10);
		sliderDEC.setPaintTicks(true);
		sliderDEC.setPreferredSize(sliderDimension);
		this.processDEC = true;
		sliderDEC.addChangeListener(this);

		
		sliderZenith = new JSlider(JSlider.VERTICAL, 0, 180, 0);
		sliderZenith.setMajorTickSpacing(90);
		sliderZenith.setMajorTickSpacing(10);
		sliderZenith.setPaintTicks(true);
		sliderZenith.setPreferredSize(sliderDimension);
		this.processZenith = true;
		sliderZenith.addChangeListener(this);
		
		world = new World();
		world.setAmbientLight(128, 128, 128);

		TextureManager.getInstance().addTexture("box", new Texture("images/box.jpg",false));
		TextureManager.getInstance().addTexture("MetalBare", new Texture("images/MetalBare0144_1_thumbhuge.jpg",false));
		TextureManager.getInstance().addTexture("MetalFloor", new Texture("images/MetalFloorsPainted0005_1_thumbhuge.jpg",false));
		TextureManager.getInstance().addTexture("ConcreteBunker", new Texture("images/ConcreteBunker0764_1_thumbhuge.jpg",false));
		cwA = cwAngleLast = (TO_RADIANS * 180);;
		declination = lastangleDEC = 0;
		CreaObservatorio();

		world.getCamera().setPosition(50, -50, -5);
		world.getCamera().lookAt(this.ascencionRecta.getTransformedCenter());
		socket = new DatagramSocket(19000);
		
		socketTimer = new javax.swing.Timer(1000, this);
		socketTimer.setActionCommand("socketTimer");
		jcbChase500 = new JCheckBox("Chase500");
		jcbChase500.setActionCommand("jcbChase500");
		jcbChase500.addActionListener(this);
		
	}

	/**
	 * Crea los objetos 3D, cada uno con su respectiva:
	 * posicion
	 * orientación
	 * Colores y Texturas
	 * Anidación: Vínculos padre-hijo que permiten modelar los ejes, que se mueven en bloque.
	 */
	private void CreaObservatorio() {
		this.observatorio = Primitives.getCylinder(FACES, 15.0f, 0.08f);
		// this.observatorio.translate(-25.0f, 0.0f, 0.0f);
		this.observatorio.setTexture("MetalFloor");
		this.observatorio.setEnvmapped(Object3D.ENVMAP_ENABLED);
		this.observatorio.setAdditionalColor(Color.WHITE);
		this.observatorio.build();

		Object3D pilar;
		pilar = Primitives.getBox(2.0f, 4.0f);
		pilar.translate(0.0f, -7.0f, 0.0f);
		pilar.setTexture("MetalBare");
		pilar.setEnvmapped(Object3D.ENVMAP_ENABLED);
		pilar.setAdditionalColor(Color.DARK_GRAY);
		pilar.build();

		Object3D topePilar;
		topePilar = Primitives.getCylinder(FACES, 3.0f, 2.0f);
		topePilar.rotateX(PI_HALF - LAT_OBSERVATORY);
		topePilar.translate(0.0f, -7.0f, 3.0f);
		topePilar.setTexture("MetalBare");
		topePilar.setEnvmapped(Object3D.ENVMAP_ENABLED);
		topePilar.setAdditionalColor(Color.YELLOW);
		topePilar.build();

		this.ascencionRecta = Primitives.getCylinder(FACES, 1.0f, 12.0f);
		this.ascencionRecta.rotateX(PI_HALF);
		this.ascencionRecta.translate(0.0f, -3.0f, 0.0f);
		this.ascencionRecta.setTexture("box");
		this.ascencionRecta.setEnvmapped(Object3D.ENVMAP_ENABLED);
		this.ascencionRecta.setAdditionalColor(Color.RED);
		this.ascencionRecta.build();

		Object3D contrapeso;
		contrapeso = Primitives.getSphere(FACES, 3.0f);
		contrapeso.translate(0.0f, -9.0f, 0.0f);
		contrapeso.setTexture("ConcreteBunker");
		contrapeso.setEnvmapped(Object3D.ENVMAP_ENABLED);
		contrapeso.setAdditionalColor(Color.GRAY);
		contrapeso.build();

		this.telescopio = Primitives.getCylinder(FACES, 2.0f, 7.0f);
		this.telescopio.rotateX(PI_HALF);
		this.telescopio.translate(0.0f, 9.0f, TELESCOPE_SHIFT);
		this.shiftZ = TELESCOPE_SHIFT;
		this.lastShiftZ = TELESCOPE_SHIFT;
		this.shiftX = this.lastShiftX = 0;
		this.telescopio.setAdditionalColor(Color.BLUE);
		this.telescopio.setTexture("box");
		this.telescopio.setEnvmapped(Object3D.ENVMAP_ENABLED);
		this.telescopio.build();

		this.observatorio.addChild(pilar);
		pilar.addChild(topePilar);
		topePilar.addChild(this.ascencionRecta);
		this.ascencionRecta.addChild(contrapeso);
		this.ascencionRecta.addChild(this.telescopio);

		world.addObject(this.observatorio);
		world.addObject(pilar);
		world.addObject(topePilar);
		world.addObject(this.ascencionRecta);
		world.addObject(contrapeso);
		world.addObject(this.telescopio);
	}

	private void loop() throws Exception {
		buffer = new FrameBuffer(800, 600, FrameBuffer.SAMPLINGMODE_NORMAL);
		Canvas canvas = buffer.enableGLCanvasRenderer();
		canvas.addMouseListener(this);
		canvas.addMouseMotionListener(this);
		buffer.disableRenderer(IRenderer.RENDERER_SOFTWARE);
		frame.setLayout(new FlowLayout());
		frame.add(canvas, BorderLayout.CENTER);
		frame.add(jcbChase500, BorderLayout.BEFORE_FIRST_LINE);
		JPanel panelControl;
		panelControl = new JPanel(new FlowLayout());
		panelControl.add(this.sliderCWa);
		panelControl.add(sliderDEC);
		panelControl.add(sliderZenith);
		frame.add(panelControl, BorderLayout.EAST);
		frame.pack();

		while (frame.isShowing()) {
			buffer.clear(java.awt.Color.lightGray);
			world.renderScene(buffer);
			world.draw(buffer);
			buffer.update();
			buffer.displayGLOnly();
			canvas.repaint();
			Thread.sleep(10);
		}
		buffer.disableRenderer(IRenderer.RENDERER_OPENGL);
		buffer.dispose();
		frame.dispose();
		System.exit(0);
	}

	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	public void mouseEntered(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	public void mouseExited(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	public void mousePressed(MouseEvent e) {
		// set the "previous" mouse location
		// this prevent the gear from jerking to the new angle
		// whenever mouseDragged gets called
		prevMouseX = e.getX();
		prevMouseY = e.getY();
	}

	public void mouseReleased(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	public void mouseDragged(MouseEvent e) {
		// here we want to rotate the gear based on the mouse dragging
		int x = e.getX();
		int y = e.getY();
		Dimension size = e.getComponent().getSize();

		float thetaX = (float) Math.PI
				* ((float) (x - prevMouseX) / (float) size.width);
		float thetaY = (float) Math.PI
				* ((float) (prevMouseY - y) / (float) size.height);

		prevMouseX = x;
		prevMouseY = y;
		observatorio.rotateY(-thetaX);
		observatorio.rotateZ(thetaY);
	}

	public void mouseMoved(MouseEvent e) {
		// TODO Auto-generated method stub

	}

	public void stateChanged(ChangeEvent e) {
		if (e.getSource() == this.sliderCWa) {
			System.out.println("---------<sliderCWa>------------");
			float valor;
			valor = this.sliderCWa.getValue();
			float angulo;
			float diff;
			angulo = (TO_RADIANS * valor);
			cwA = (angulo);

			diff = (cwAngleLast - cwA);
			System.out.println("cwAngle=" + valor + " diff=" + diff);

			this.ascencionRecta.rotateY(diff);
			cwAngleLast = cwA;		
			this.refrescaZenithSlider();
			System.out.println("---------</sliderCWa>------------");
		}
		if  ((e.getSource() == this.sliderDEC)) {
			System.out.println("---------<sliderDEC>------------");
			float valor;
			valor = this.sliderDEC.getValue();
			float angulo;
			float diff, difX, difZ;
			angulo = (TO_RADIANS * valor);
			declination = (angulo);

			diff = (lastangleDEC - declination);
			System.out.println("DEC=" + valor + " valor=" + diff);

			shiftX = (float) (Gem3D.TELESCOPE_SHIFT * Math.sin(angulo));
			shiftZ = (float) (Gem3D.TELESCOPE_SHIFT * Math.cos(angulo));
			difX = lastShiftX - shiftX;
			difZ = lastShiftZ - shiftZ;
			telescopio.translate(difX, 0.0f, -difZ);
			this.telescopio.rotateY(diff);
			this.lastangleDEC = this.declination;
			this.lastShiftX = shiftX;
			this.lastShiftZ = shiftZ;
			if (processDEC)
			{
				processZenith = false;
				this.refrescaZenithSlider();
				processZenith = true;
			}
			System.out.println("---------</sliderDEC>------------");
		}
		if ( (e.getSource() == this.sliderZenith)) {
			System.out.println("---------<sliderZenith>------------");
			double zenith;
			zenith = this.sliderZenith.getValue();
			System.out.println("Zenith=" + zenith );	
			double cwa_rad;
			cwa_rad = (this.sliderCWa.getValue() * Gem3D.TO_RADIANS);
			double dec_rad;
			dec_rad = computeDeclinationAngle( cwa_rad , zenith * Gem3D.TO_RADIANS);
			if ( Double.isNaN(dec_rad))
			{
			 this.sliderZenith.setBackground(Color.BLACK);
			}else 
			{
				int dec_grad;
				dec_grad = (int) (dec_rad * TO_GRAD);
				if (processZenith)
				{
					this.processDEC = false;
					this.sliderDEC.setValue(dec_grad);
					this.processDEC = true;
				}
				this.sliderZenith.setBackground(Color.WHITE);
			}

			System.out.println("---------</sliderZenith>------------");
		}
	}
	
	public void refrescaZenithSlider ()
	{
		this.decV = computeDeclination((float)this.cwA, (float)this.declination);
		float zenith_local;
		zenith_local = this.decV.calcAngle(ZENITH);
		int zenith_i;
		zenith_i = (int) (zenith_local * TO_GRAD);
		System.out.println("decV="+decV.toString()+"\t Zenih="+(zenith_i));
		this.processZenith = false;
		this.sliderZenith.setValue(zenith_i);
		this.processZenith = true;			
	}
	
	/**
	 * Calcula el vector unitario de declinación para un telescopio situado en 
	 * la latitud definida por LAT_OBSERVATORY.
	 * No se utilizan las variable de instancia de esta clase pues la idea es comprobar calculos
	 * a partir de los resultados de esta funcion.
	 * @param _cwa
	 * @param _declination
	 * @return Vector Unitario de Declinacion
	 */
	private static SimpleVector computeDeclination(float _cwa, float _declination)
	{
		SimpleVector _declinationV;
		_declinationV = new SimpleVector();
		_declinationV.x = (float) (Math.sin(LAT_OBSERVATORY_COMP) * Math.cos(_declination) - 
		Math.cos(LAT_OBSERVATORY_COMP) * Math.sin(_cwa) * Math.sin(_declination));
		_declinationV.y = (float) (Math.cos(_cwa) * Math.sin(_declination));
		_declinationV.z = (float) (Math.cos(LAT_OBSERVATORY_COMP) * Math.cos(_declination) + 
				Math.sin(LAT_OBSERVATORY_COMP) * Math.sin(_cwa) * Math.sin(_declination));
		return _declinationV;
	}
	
	/**
	 * Calcula un angulo de declinacion que sea coherente con los angulos recibidos en los parámetros.
	 * @param cwa Angulo del contrapeso en radianes
	 * @param zenith Angulo del Zenith en radianes
	 * @return angulo de Declinacion en radianes
	 */
	public double computeDeclinationAngle (double cwa, double zenith)
	{
		double respuesta;
		double alpha, beta;
		alpha = (   Math.cos(LAT_OBSERVATORY_COMP) / Math.cos(zenith));
		beta  = ( ( Math.sin(LAT_OBSERVATORY_COMP) * Math.sin(cwa) ) / Math.cos(zenith));
		Vector2f quadratic;
		double a;
		double b;
		double c;
		a = ( (alpha * alpha) + (beta * beta) );
		b = (-2.0 * beta);
		c = (1.0 - (alpha * alpha));
		quadratic = SecondDegree(a, b, c);
		System.out.println("x1="+quadratic.x+"\t x2="+quadratic.y+"\t delta="+(quadratic.x - quadratic.y)+"\t alpha="+alpha+"\t beta="+beta);
		
		respuesta = Double.NaN;
		float solA; // Primera solucion de la ecuacion de segundo grado
		float solB; // Segunda solucion de la ecuacion de segundo grado
		
		solA = quadratic.x;
		solB = quadratic.y;
		List<DeclinationError> decErrorList;
		decErrorList = new ArrayList<DeclinationError>();
		//Si la solucion esta en el recorrido de la funcion seno
		if ((solA!=Float.NaN) && (solA>=-1) && (solA <=1))
		{
			double angleA;
			angleA = Math.asin(solA) - 2 * Math.PI;
			for (int i=-2;i<3;i++)
			{	
				if  ((angleA>= (-1 *Math.PI)) && (angleA<= (Math.PI)))
				{
					DeclinationError deA1;
					deA1 = evaluaDeclinacion(angleA, zenith);
					decErrorList.add(deA1);
				}
				angleA+=Math.PI;
			}
		}
		//Si la solucion esta en el recorrido de la funcion seno
		if ((solB!=Float.NaN) && (solB>=-1) && (solB <=1))
		{
			double angleB;
			angleB = Math.asin(solB) - 2 * Math.PI;
			for (int i=-2;i<3;i++)
			{	
				if  ((angleB>= (-1 *Math.PI)) && (angleB<= (Math.PI)))
				{
					DeclinationError deB1;
					deB1 = evaluaDeclinacion(angleB, zenith);
					decErrorList.add(deB1);
				}
				angleB+=Math.PI;
			}			 
		}
		int largoLista;
		int minIndex=0;
		Collections.shuffle(decErrorList);
		Collections.sort(decErrorList);
		largoLista = decErrorList.size();
		if (largoLista>0)
		{
			DeclinationError deLocal;
			for (int i=0;i<largoLista;i++)
			{
				deLocal = decErrorList.get(i);
				System.out.println("deLocal["+i+"].error="+deLocal.getError());			
			}
			respuesta = decErrorList.get(minIndex).getAngle();
		}
		return respuesta;		
	}
	
	public DeclinationError evaluaDeclinacion (double declinationTest, double expectedZenith)
	{
		DeclinationError respuesta;
		SimpleVector decVector;
		decVector = computeDeclination(this.cwA, (float) declinationTest);
		double error;
		error = Math.abs(decVector.calcAngle(ZENITH) - expectedZenith); // Error al suponer que solA es la respuesta correcta
		respuesta = new DeclinationError(declinationTest, error);
		System.out.println("errorB="+error);
		return respuesta;
	}
	
	public static Vector2f SecondDegree (double a,double b,double c)
	{
	 Vector2f respuesta;
	 respuesta = new Vector2f(0, 0);
	 double discriminant;
	 discriminant = ( (b * b) - (4.0 * a * c));
	 if (discriminant<0)
	 {
	  System.err.println("0>discriminant="+discriminant);
	 }
	 respuesta.x = (float) (((-1.0 * b) + Math.sqrt(discriminant)) / (2.0 * a));
	 respuesta.y = (float) (((-1.0 * b) - Math.sqrt(discriminant)) / (2.0 * a));
	 return respuesta;	 
	}

	@Override
	public void actionPerformed(ActionEvent arg0) {
		String action;
		action=arg0.getActionCommand();
		if (action.equals("jcbChase500"))
		{
			if (this.jcbChase500.isSelected())
			{
			 socketTimer.start();
			}else
			{
			 socketTimer.stop();
			}
		}
		if (action.equals("socketTimer"))
		{
			DatagramPacket packetSend;
			
			String mensaje;
			packetSend = null;
			mensaje = null;
			try {
				packetSend = new DatagramPacket(new byte[1],1,InetAddress.getByName(Gem3D.UdpServerHost),Gem3D.UdpServerPort);
			} catch (UnknownHostException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
			
			try {
				this.socket.send(packetSend);
				byte[] bbuffer;
				DatagramPacket packetReceive;
				bbuffer = new byte[50];
				packetReceive = new DatagramPacket(bbuffer, 50);
				this.socket.receive(packetReceive);
				
				mensaje = new String(packetReceive.getData());
				//System.out.println("mensaje=\t"+mensaje);
				mensaje = mensaje.trim().replaceAll("_", "");
			
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			double cwA_Baade,zenithAngle_Baade;
			int cwaSet,zSet;
			String[] part;
			//System.out.println("mensaje=\t"+mensaje);
			part = mensaje.split(" ");
			cwA_Baade = Double.parseDouble(part[0]);
			zenithAngle_Baade = Double.parseDouble(part[1]);
			cwaSet = (int) (cwA_Baade);
			zSet = (int) zenithAngle_Baade;
			System.out.println("mensaje=("+mensaje+")\t cwaSet="+cwaSet+"\t zSet"+zSet);
			this.sliderCWa.setValue(cwaSet);
			this.sliderZenith.setValue(zSet);
		}	
		
	}	
}