/* jpvmException.java
 *
 * A throwable class representing a jpvm run-time error.
 *
 * Adam J Ferrari
 * Sat 05-25-1996
 *
 * Copyright (C) 1996  Adam J Ferrari
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Library General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Library General Public License for more details.
 * 
 * You should have received a copy of the GNU Library General Public
 * License along with this library; if not, write to the
 * Free Software Foundation, Inc., 675 Mass Ave, Cambridge,
 * MA 02139, USA.
 */

package jpvm;

public
class jpvmException extends Throwable {
	private String val;

	public jpvmException() {
		val = "jpvm error: unknown error.";
	}

	public jpvmException(String str) {
		val = new String("jpvm error: "+str);
	}

	public String toString() {
		return new String(val);
	}
};
