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
query_ohlc = '''
SELECT Date, OpenPrice, HighPrice, LowPrice, ClosePrice
FROM HistoricalData
WHERE InstrumentId = ? AND YEAR(Date) < 2010 
ORDER BY Date
'''

# Fetch OHLC data into a DataFrame
df_ohlc = pd.read_sql(query_ohlc, conn, params=[insId])

# Parameterized query to fetch Positions data with Created date before 2010
query_positions = '''
SELECT Created, ClosedAt, EntryPrice, ClosePrice, PositionType
FROM Positions
WHERE InstrumentId = ? and TestId = 1
AND YEAR(Created) < 2010
ORDER BY Created
'''

# Fetch Positions data into a DataFrame
df_positions = pd.read_sql(query_positions, conn, params=[insId])

# Close the connection
conn.close()

# Create OHLC chart with dynamic y-axis range
fig = go.Figure(data=[go.Ohlc(x=df_ohlc['Date'],
                              open=df_ohlc['OpenPrice'],
                              high=df_ohlc['HighPrice'],
                              low=df_ohlc['LowPrice'],
                              close=df_ohlc['ClosePrice'])])

# Overlay trades from Positions table with color-coding based on profit/loss
for index, row in df_positions.iterrows():
    
    if row['PositionType'] == 0 and row['ClosePrice'] > row['EntryPrice']:
     color = 'pink'
    elif row['PositionType'] == 0 and row['ClosePrice'] < row['EntryPrice']:
     color = 'blue'
    elif row['PositionType'] == 1 and row['ClosePrice'] < row['EntryPrice']:
     color = 'pink'
    elif row['PositionType'] == 1 and row['ClosePrice'] > row['EntryPrice']:
     color = 'blue'

    fig.add_trace(go.Scatter(
        x=[row['Created'], row['ClosedAt']],
        y=[row['EntryPrice'], row['ClosePrice']],
        mode='lines+markers',
        line=dict(color=color),
        name=f"Trade {index+1}"
    ))

# Add title and labels
fig.update_layout(
    title='OHLC Chart with Trades from SQL Server Data',
    xaxis_title='Date',
    yaxis_title='Price',
    xaxis_rangeslider_visible=True,
    yaxis=dict(autorange=True)
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
fig.write_html("ohlc_chart_with_trades.html", include_plotlyjs='cdn')

print("OHLC chart with trades created and saved as ohlc_chart_with_trades.html")
