package examples;

import jpvm.jpvmBuffer;
import jpvm.jpvmEnvironment;
import jpvm.jpvmException;
import jpvm.jpvmMessage;
import jpvm.jpvmTaskId;

public class exemplo_jpvm {

	static int num_workers = 4; 
	
    public static void main(String[] args) {
		try {
			jpvmEnvironment jpvm = new jpvmEnvironment();
			
			jpvmTaskId myid = jpvm.pvm_mytid();
			jpvmTaskId masterTaskId = jpvm.pvm_parent();
			System.out.println("task id = " + myid.toString());
			
			//definir se eh o master, ou se eh algum escravo.
			if (masterTaskId==jpvm.PvmNoParent) {
				//é o master
				System.out.println("inicializado como master");
				jpvmTaskId tids[] = new jpvmTaskId[num_workers];
				jpvm.pvm_spawn("examples.exemplo_jpvm", num_workers, tids);
				
				System.out.println("workers taks: ");
				int i;
				for (i=0; i < num_workers; i++)
					System.out.println("\t" + tids[i].toString());
				
				for (i=0; i < num_workers; i++) {
					System.out.println("mandando mensagem para o worker " + i);
					jpvmBuffer buf = new jpvmBuffer();
					buf.pack("servidor, id: " + jpvm.pvm_mytid().toString());
					jpvm.pvm_send(buf, tids[i], i);
					
					System.out.println("recebeu resposta:");
					jpvmMessage message = jpvm.pvm_recv();
					String str = message.buffer.upkstr();
					System.out.println("recebeu : " + str);
					System.out.println("com a tag " + 
							message.messageTag + " from " + 
							message.sourceTid.toString());
				}
				
			} else {
				//é o escravo.
				System.out.println("inicializado como escravo.");
				
				jpvmMessage message = jpvm.pvm_recv();
				String str = message.buffer.upkstr();
				System.out.println("recebeu : " + str);
				System.out.println("com a tag " + 
						message.messageTag + " from " + 
						message.sourceTid.toString());
				
				System.out.println("preparando resposta.");
				jpvmBuffer buf = new jpvmBuffer();
				buf.pack(str + " cliente : " + jpvm.pvm_mytid().toString());
				jpvm.pvm_send(buf, masterTaskId, 0);
				System.out.println("mando resposta.");
			}
			jpvm.pvm_exit();
		} catch (jpvmException e) {
			e.printStackTrace(System.out);
		}

	}

}
