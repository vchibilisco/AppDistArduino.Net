/********************************************************************************************* 
 * 
 * Tutorial: Robot Sumo
 * Autor: Vicente Chibilisco
 * 
 ********************************************************************************************/
//Añado librerias
#include<MotorRed.h>
#include<Ultrasonic.h>

//Declaro constantes

//Motor Derecha
#define MOTOR1_CTL1  8  // I1  
#define MOTOR1_CTL2  9  // I2  
#define MOTOR1_PWM   11 // EA  
//Motor Izquierda
#define MOTOR2_CTL1  6  // I3  
#define MOTOR2_CTL2  7  // I4  
#define MOTOR2_PWM   10 // EB  
//Direccion (adelante, atras)
#define MOTOR_DIR_FORWARD  0  
#define MOTOR_DIR_BACKWARD   1  
//Radio de zona de lucha
#define RADIO_ZL 20
//Velocidad de giro
#define VELOCIDAD_GIRO 200
//Tring
#define TRING 2
//Echo
#define ECHO 3

//Ultrasonido, constantes de pin de placa
Ultrasonic ultraSo(TRING,ECHO); //(tring,echo) 
//Motor Reductor, constantes de pin de placa

MotorRed motor(MOTOR1_CTL1, MOTOR1_CTL2, MOTOR1_PWM, MOTOR2_CTL1, MOTOR2_CTL2, MOTOR2_PWM);

//Declaro variables

//Bandera que indica en que sentido giraran los motores
boolean banderaGiro;
//Distancia tomada por el ultrasonido
long distancia;
long threadSleep;

char rxChar; // Variable para recibir datos del puerto serie

char ssid[10]		= "Arduino1";	// Nombre para el modulo Bluetooth.
char baudios		 = '4';		   // 1=>1200 baudios, 2=>2400, 3=>4800, 4=>9600 (por defecto), 5=>19200, 6=>38400, 7=>57600, 8=>115200
char password[10]	= "0001";		// Contraseña para el emparejamiento del modulo.


void setup()  
{  
  Serial1.begin(9600);

  banderaGiro = false;
  distancia = 0.0;
  delay(5000);
  // Se inicia la configuración:
  Serial1.print("AT"); 
  delay(1000);

  // Se ajusta el nombre del Bluetooth:
  Serial1.print("AT+NAME"); 
  Serial1.print(ssid); 
  delay(1000);

  // Se ajustan los baudios:
  Serial1.print("AT+BAUD"); 
  Serial1.print(baudios); 
  delay(1000);

  // Se ajusta la contraseña:
  Serial1.print("AT+PIN"); 
  Serial1.print(password); 
  delay(1000);

  threadSleep = 200;
}  

void loop()
{    
  //Lee la distancia desde el Ultrasonido
  distancia = ultraSo.Ranging(CM);

  //Si la distancia captada por el Ultrasonido es menor al
  //radio de la zona de lucha, inicia los motores hacia adelante
  Serial1.println(distancia); // CM or INC
  sleep();
  
  if(Serial1.available()){
    rxChar = Serial1.read();
  }
  else{
    rxChar= '%';
  }
  
  switch(rxChar){
  case 'w':
    Adelante();
    sleep();
    break;
  case 'a' :
    GiroAntiHorario();
    sleep();
    break; 
  case 'd':
    GiroHorario();
    sleep();
    break; 
  case 's':
    Atras();
    sleep();
    break;
  default:
    Paro();
    sleep();
    break;
  }
}

//Adelante
void Adelante(){
  //Adelante
  motor.startMotor1(MOTOR_DIR_BACKWARD,VELOCIDAD_GIRO);

  //Motor izquierdo gira hacia adelante, VELOCIDAD_GIRO
  motor.startMotor2(MOTOR_DIR_FORWARD,VELOCIDAD_GIRO);
}

//Giro Horario
void GiroHorario(){
  //El robot gira en sentido horario
  //Motor derecho gira hacia atras, VELOCIDAD_GIRO
  motor.startMotor1(MOTOR_DIR_FORWARD,VELOCIDAD_GIRO);

  //Motor izquierdo gira hacia adelante, VELOCIDAD_GIRO
  motor.startMotor2(MOTOR_DIR_FORWARD,VELOCIDAD_GIRO);
}

//Giro AntiHorario
void GiroAntiHorario(){
  //El robot gira en sentido anti horario
  //Motor derecho gira hacia adelante, VELOCIDAD_GIRO
  motor.startMotor1(MOTOR_DIR_BACKWARD,VELOCIDAD_GIRO);

  //Motor izquierdo gira hacia atras, VELOCIDAD_GIRO
  motor.startMotor2(MOTOR_DIR_BACKWARD, VELOCIDAD_GIRO);
}

//Paro
void Paro(){
  motor.stopMotor1();
  motor.stopMotor2();
}

//Atras
void Atras(){
  //Adelante
  motor.startMotor1(MOTOR_DIR_FORWARD,VELOCIDAD_GIRO);

  //Motor izquierdo gira hacia adelante, VELOCIDAD_GIRO
  motor.startMotor2(MOTOR_DIR_BACKWARD,VELOCIDAD_GIRO);
}

void sleep(){
  delay(threadSleep);
}
