import pandas as pd
import pyodbc
import plotly.graph_objects as go

# SQL Server connection details
server = 'localhost'
database = 'Experiment'
username = 'sa'
password = 'yourStrongPassword'
insId = 1

# Establish connection to SQL Server
conn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};'
                      'SERVER=' + server + ';'
                      'DATABASE=' + database + ';'
                      'UID=' + username + ';'
                      'PWD=' + password)

# Parameterized query to fetch OHLC data
query = '''
SELECT Date, OpenPrice, HighPrice, LowPrice, ClosePrice
FROM HistoricalData
WHERE InstrumentId = ?
ORDER BY Date
'''

# Fetch data into a DataFrame
df = pd.read_sql(query, conn, params=[insId])

# Close the connection
conn.close()

# Create OHLC chart with dynamic y-axis range
fig = go.Figure(data=[go.Ohlc(x=df['Date'],
                              open=df['OpenPrice'],
                              high=df['HighPrice'],
                              low=df['LowPrice'],
                              close=df['ClosePrice'])])

# Add title and labels
fig.update_layout(
    title='OHLC Chart from SQL Server Data',
    xaxis_title='Date',
    yaxis_title='Price',
    xaxis_rangeslider_visible=True
)

# JavaScript to dynamically update y-axis range
fig.update_layout(
    xaxis=dict(
        rangeselector=dict(
            buttons=list([
                dict(count=1, label="1m", step="month", stepmode="backward"),
                dict(count=6, label="6m", step="month", stepmode="backward"),
                dict(step="all")
            ])
        ),
        rangeslider=dict(visible=True),
        type="date"
    ),
    yaxis=dict(fixedrange=False)
)

# Save the figure as an HTML file with embedded JavaScript
fig.write_html("ohlc_chart.html", include_plotlyjs='cdn')

print("OHLC chart created and saved as ohlc_chart.html")
