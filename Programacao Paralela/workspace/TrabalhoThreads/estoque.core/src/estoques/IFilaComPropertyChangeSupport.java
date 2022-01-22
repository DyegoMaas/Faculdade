package estoques;

import java.beans.PropertyChangeListener;

public interface IFilaComPropertyChangeSupport extends IFila {

	void addPropertyChangeListener(PropertyChangeListener l);

	void removePropertyChangeListener(PropertyChangeListener l);
}

