import sys
import mysql.connector
import tkinter as tk
import tkinter.ttk as ttk

mydb = mysql.connector.connect(
  host="192.168.198.147",
  user="root",
  passwd="123456",
  database="DB1"
)

curs=mydb.cursor()




    
##############################################################
#
#                        Essential task
#
##############################################################

def essTask():
    command=input("Input type(1 - Details_Knot, 2 - Details_Informations, join)\n")

    if command=='1':
        curs.execute("select * from Details_Knot")
    if command=='2':
        curs.execute("select * from Details_Informations")
    if command=='3':
        curs.execute("select * from Details_Knot inner join Details_Informations on Details_Informations.Descripton = Details_Knot.ID_Description")

    reader=curs.fetchall()

    for r in reader:
        for j in range(0,len(r)):
            print('%14s' % r[j], end == " ")
        print("\n")

##############################################################
#
#                           End
#
##############################################################

##############################################################
# 
#                       Visual interface
# 
##############################################################    

#tkinter table  
class Table(tk.Frame):
    def __init__(self, parent=None, headings=tuple(), rows=tuple()):
        super().__init__(parent)
  
        table = ttk.Treeview(self, show="headings", selectmode="browse")
        table["columns"]=headings
        table["displaycolumns"]=headings
  
        for head in headings:
            table.heading(head, text=head, anchor=tk.CENTER)
            table.column(head, anchor=tk.CENTER,width=120)
  
        for row in rows:
            table.insert('', tk.END, values=tuple(row))
  
        scrolltable = tk.Scrollbar(self, command=table.yview)
        table.configure(yscrollcommand=scrolltable.set)
        scrolltable.pack(side=tk.RIGHT, fill=tk.Y)
        table.pack(expand=tk.YES, fill=tk.BOTH)
   
def readKnot():
    global table
    curs.execute("select * from Details_Knot")

    res=[]
    reader=curs.fetchall()
    for r in reader:
        res.append(r)
    table.destroy()
    table = Table(frame1, headings=('ID', 'Name_Datail', 'Seria_Number','ID_Description','Manufacture','Material'), rows=res)
    table.pack(expand=tk.YES, fill=tk.BOTH)

def readDetails_Informations():
    global table
    curs.execute("select * from Details_Informations")

    res=[]
    reader=curs.fetchall()
    for r in reader:
        res.append(r)
    table.destroy()
    table = Table(frame1, headings=('ID', 'Model' , 'Description'), rows=res)
    table.pack(expand=tk.YES, fill=tk.BOTH)

def readJoin():
    global table
    curs.execute("select * from Details_Knot inner join Details_Informations on Details_Informations.Descripton = Details_Knot.ID_Description")

    res=[]
    reader=curs.fetchall()
    for r in reader:
        res.append(r)
    table.destroy()
    table = Table(frame1, headings=('ID', 'Name_Datail', 'Seria_Number','ID_Description','Manufacture','Material', 'Model' , 'Description'), rows=res)
    table.pack(expand=tk.YES, fill=tk.BOTH)


def visualRun():

    global table

    global frame

    global frame1

    root = tk.Tk()

    frame=tk.Frame(root)
    frame.pack()

    frame1=tk.Frame(root)
    frame1.pack(side=tk.BOTTOM)

    table = Table(frame1, headings=('ID', 'Name_Datail', 'Seria_Number','ID_Description','Manufacture','Material'), rows=[])
    table.pack(expand=tk.YES, fill=tk.BOTH)

    bluebutton = tk.Button(frame, text = "Select Details_Knot", command=readKnot)
    bluebutton.pack( side = tk.LEFT )

    bluebutton = tk.Button(frame, text = "Select Details_Informations", command=readDetails_Informations)
    bluebutton.pack( side = tk.LEFT )

    bluebutton = tk.Button(frame, text = "Joined table", command=readJoin)
    bluebutton.pack( side = tk.LEFT )

    root.mainloop()

##############################################################
#
#                    End visual interface
#
##############################################################


##############################################################
#
#                           CLI mode
#
##############################################################

#print table from Database to console
def printTable(head):
    reader=curs.fetchall()
    for j in range(0,len(head)):
        print('%14s' % head[j], end == " ")
    print("\n")

    for r in reader:
        for j in range(0,len(r)):
            print('%14s' % r[j], end == " ")
        print("\n")

def insertInformations():
    model=input('\nModel: ')
    description=input('\nDescription: ')    

    query = "insert into Details_Informations(Model, Description) values(n'{model}', n'{description}')"
    curs.execute(query)

    mydb.commit()

#print table informations
def showAm():
    curs.execute("select * from Details_Informations")   

    printTable(('ID', 'Model' , 'Description'))

#print table Knot
def showEnt():
    curs.execute("select * from Details_Knot")
    printTable(('ID', 'Name_Datail', 'Seria_Number','ID_Description','Manufacture','Material'))

def showJoin():
    curs.execute("select * from Details_Knot inner join Details_Informations on Details_Informations.Descripton = Details_Knot.ID_Description")    
    printTable(('ID', 'Name_Datail', 'Seria_Number','ID_Description','Manufacture','Material','ID', 'Model' , 'Description'))

#delete data from informations
def deleteAm():
    id=input('ID: ')
    query="delete from Descriptions_Informatioms where ID = {id}"
    curs.execute(query)
    mydb.commit()
    print('record id={id} has been deleted successfully\nWARNING: if this record doesn`t exist this record wont be deleted:)')
    
def deleteEnt():
    id=input("ID: ")
    query= "delete Details_Knot where ID = {id}"
    curs.execute(query)
    mydb.commit()
    print('record id={id} has been deleted successfully\nWARNING: if this record doesn`t exist this record wont be deleted:)')
    
def updatINF():
    id_ent=input("ID: ")
    model=input("Model: ")
    description=input("Description: ")
    query="update Details_Informations set Model = {model}, Description = n'{description}' where ID={id_ent}"
    curs.execute(query)
    mydb.commit()
    print('record id={id_ent} has been updated successfully\nWARNING: if this record doesn`t exist this record wont be updated:)')
    
def updateEnt():
    recordId=input("ID: ")
    detName=input("Name_Detail: ")
    detSeriar=input("Seria_Number: ")
    detDescript=input("ID_Description: ")
    manuf=input("Manufacture: ")
    material=input("Material: ")

    query="update Details_Knot set Name_Detail = n'{detName}', Seria_Number = n'{detSeria}', ID_Description = n'{detDescript}', Manufacture = {price}, Material = {material} where ID = {recordId}"
    curs.execute(query)
    mydb.commit()
    print('record id={id} has been updated successfully\nWARNING: if this record doesn`t exist this record wont be updated:)')
    
def insertKnt():
    detName=input("Detail name: ")
    detSeria=input("Seria : ")
    detDescript=input("Description: ")
    manuf=input("Manufacture: ")
    material=input("Material: ")

    query="insert into Details_Knot(Name_Detail, Seria_Number, ID_Description, Manufacture, Material) values(n'{detName}, n'{detSeria}', n'{detDescript}', n'{manuf}', n'{material}')"
    curs.execute(query)
    mydb.commit()
    print('record has been inserted successfully\n')
    

def runCli():
    while True:
        command=input("Task3 CLI > ")
        if command=='help':
            print('''Task3 CLI help:\n
        showam: \tshow Details_Informations
        showent: \tshow trading enterprises
        showjoin: \tshow joined table
        --------------------------------------------
        delam: \t\tdelate amount of trade    
        delent: \tdelate trading enterprise
        --------------------------------------------
        updam: \t\tupdate amount of trade
        updent \t\tupdate enterprice
        --------------------------------------------
        insam: \t\tinsert into Details_Informations
        insent: \tinsert into enterprises
        --------------------------------------------
        exit: \t\texit to command line
            ''')
        
        if command=='showam':
            showAm()

        if command=='showent':
            showEnt()

        if command=='showjoin':
            showJoin()

        if command=='delam':
            deleteAm()

        if command=='delent':
            deleteEnt()

        if command=='updam':
            updatINF()

        if command=='updent':
            updateEnt()

        if command=='insam':
            insertInformations()

        if command=='insent':
            insertKnt()
    
        if command=='exit':
            break

##############################################################
#
#                          End CLI mode
#
##############################################################

if __name__=='__main__':    
    if len(sys.argv)==1:
        essTask()
    elif sys.argv[1]=="-v":
        visualRun()
    elif sys.argv[1]=='-c':
        runCli()
