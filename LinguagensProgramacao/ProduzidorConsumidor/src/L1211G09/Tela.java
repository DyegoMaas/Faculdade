package L1211G09;

import java.awt.GridLayout;
import java.util.Observable;
import java.util.Observer;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextArea;

@SuppressWarnings("serial")
public class Tela extends JFrame implements Observer{

	private Produtor[] produtores;
	private Consumidor[] consumidores;

    private JTextArea texto = new JTextArea ();  
      
    public Tela () {  
      super ("Produtores x Consumidores");        
      
      this.inicializar();      
      this.montarLeiaute();   
      
      produtores[0].run();
    }

	private void inicializar() {
		while(true){
		  try{
			  int qtdProdutores = Integer.parseInt(JOptionPane.showInputDialog("Informe o número de produtores:"));
			  int tamArmazem = Integer.parseInt(JOptionPane.showInputDialog("Informe o número de áreas para armazenar produtos enviados pelos produtores:"));
			  int qtdConsumidores = Integer.parseInt(JOptionPane.showInputDialog("Informe o número de consumidores:"));
			  
			  Armazem.create(tamArmazem);
			  
			  produtores = new Produtor[qtdProdutores];
			  for(int i = 0; i < produtores.length; i++){
				  produtores[i] = new Produtor(i + 1);
				  produtores[i].addObserver(this);
			  }
			  
			  consumidores = new Consumidor[qtdConsumidores];  		  
			  
			  break;
		  }
		  catch(Exception e)
		  {    		  
			  JOptionPane.showMessageDialog(null, "Digite apenas valores numéricos inteiros :)", "Erro", JOptionPane.OK_OPTION);
			  continue;
		  }    	  
		}
	}    
      
    private void montarLeiaute () {  
    	setSize(800, 600);    	
    	setLayout(new GridLayout(1, 3, 3, 3));
    	
    	getContentPane().add(criarRepresentacaoProdutores()); 	
    }  
    
    private JPanel criarRepresentacaoProdutores() {
    	JPanel panel = new JPanel();
    	panel.setLayout(new GridLayout(produtores.length, 1, 0, 2));
    	
    	for(int i = 0; i < produtores.length; i++) {
    		Produtor produtor = produtores[i];
    		
    		JPanel panelProdutor = new JPanel();
    		panelProdutor.setLayout(new GridLayout(5, 1, 0, 2));
        	
    		panelProdutor.add(new JLabel("PRODUTOR 1." + produtor.getNumero()));
    		panelProdutor.add(new JLabel("Total produzido: " + produtor.getTotalProduzido()));
    		panelProdutor.add(new JLabel("Produto: nenhum"));
    		panelProdutor.add(new JLabel("Tempo de produção: " + produtor.getTempoProducao()));
    		panelProdutor.add(new JButton("FINALIZAR") 
        	{
        		private void execute() {
    				// TODO Auto-generated method stub
        		}
        	});
        	
    		panel.add(panelProdutor);
    	}    		
    	
    	return panel;
    }
      
    public static void main (String [] args) {
        Tela janela = new Tela();         
        janela.show();
    }

	@Override
	public void update(Observable o, Object arg) {
		if(o instanceof Produtor) {
			UpdateProdutor((Produtor)o);
		}
		else
		{
			
		}
		
	}  
	
	private void UpdateProdutor(Produtor p) {
		for(int i = 0; i < produtores.length; i++) {
			if(produtores[i] == p) {
				JPanel panel = (JPanel)((JPanel)(getContentPane().getComponent(0))).getComponent(i);
				((JLabel)panel.getComponent(1)).setText("Total produzido: " + p.getTotalProduzido());
				((JLabel)panel.getComponent(2)).setText("Produto: " + p.getUltimoProduto());
				((JLabel)panel.getComponent(3)).setText("Tempo de produção: " + p.getTempoProducao());
				
				panel.updateUI();
				break;
			}
		}
	}
}
