import javax.swing.*;
import java.awt.*;

/**
 * Created by Laura on 11-11-2016.
 */
public class MainView extends JFrame{
    private Dimension screenSize = new Dimension(500,500);
    private JPanel panel;
    private Container contentPane;

    public MainView(){
        panel = new JPanel();

        setResizable(false);
        this.setTitle("LINAL");
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setBounds(100, 100, 450, 300);
        contentPane = this.getContentPane();
//        ((JComponent) contentPane).setBorder(new EmptyBorder(5, 5, 5, 5));

        this.setPreferredSize(screenSize);

        this.setVisible(true);
        this.pack();
    }

}
