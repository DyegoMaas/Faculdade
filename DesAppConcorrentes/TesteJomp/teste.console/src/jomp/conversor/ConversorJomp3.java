package jomp.conversor;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.channels.FileChannel;

import javax.swing.JFileChooser;



import jomp.compiler.*;

public class ConversorJomp3 {

    public static void main(String[] args) {
        String fileName;
   
        JFileChooser choose = new JFileChooser();
        choose.setCurrentDirectory(new java.io.File("."));
        
        choose.showDialog(null, "Conveter...");
        fileName = choose.getSelectedFile().getPath();
        
        System.out.println("Origem - " + fileName);
        File oldFile = new File(fileName);
        
        fileName = fileName.substring(0, fileName.indexOf('.')) + "_jomp";
        File newFile = new File(fileName + ".jomp");
        
        try {
            copyFile2(oldFile, newFile);
            
        } catch (Exception e) {
            e.printStackTrace();
        }
        
        
        //compila arquivo .jomp
        String[] s = new String[1];
        s[0] = fileName;
        Jomp.main(s);
        System.out.println("Destino - " + fileName + ".java");

    }
    
    
    public static void copyFile(File sourceFile, File destFile) throws IOException {
        if(!destFile.exists()) {
         destFile.createNewFile();
        }

        FileChannel source = null;
        FileChannel destination = null;
        try {
         source = new FileInputStream(sourceFile).getChannel();
         destination = new FileOutputStream(destFile).getChannel();
         destination.transferFrom(source, 0, source.size());
        }
        finally {
         if(source != null) {
          source.close();
         }
         if(destination != null) {
          destination.close();
         }
       }
    }
    
    public static void copyFile2(File sourceFile, File destFile) throws IOException {
    	
    	String className = null;
        BufferedReader reader = new BufferedReader(new FileReader(sourceFile));
        BufferedWriter writer = new BufferedWriter(new FileWriter(destFile));

        //... Loop as long as there are input lines.
        String line = null;
        while ((line=reader.readLine()) != null) {
        	
        	if (line.contains("public class")) {
        		className = line.split(" ")[2];
        		//System.out.println(className);
        	}
        	
        	if (className != null && line.contains(className)) {
        		System.out.println("subst className " + line);
        		line = line.replace(className, className + "_jomp");
        	}
        	
            writer.write(line);
            writer.newLine();   // Write system dependent end of line.
        }

        //... Close reader and writer.
        reader.close();  // Close to unlock.
        writer.close();  // Close to unlock and flush to disk.
    }
    
    
}
    

