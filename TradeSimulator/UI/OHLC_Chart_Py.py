import pandas as pd
import pyodbc
import plotly.graph_objects as go
import locale

# SQL Server connection details
server = 'localhost'
database = 'TradingBE'
username = 'sa'
password = 'Gogogo123!'
insId = '3'
testId = '13738'
no_results_before_year_filter = 2022 

# Set locale to UK for GBP formatting
locale.setlocale(locale.LC_ALL, 'en_GB.UTF-8')

# Establish connection to SQL Server
conn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};'
                      'SERVER=' + server + ';'
                      'DATABASE=' + database + ';'
                      'UID=' + username + ';'
                      'PWD=' + password)

# Parameterized query to fetch OHLC data
query_ohlc = f'''
SELECT Date, OpenPrice, HighPrice, LowPrice, ClosePrice
FROM HistoricalData
WHERE InstrumentId = ? and YEAR(Date) >= {no_results_before_year_filter}
ORDER BY Date
'''

# Fetch OHLC data into a DataFrame
df_ohlc = pd.read_sql(query_ohlc, conn, params=[insId])

# Parameterized query to fetch Positions data with Created date before 2010
query_positions = f'''
SELECT Id, Created, ClosedAt, EntryPrice, ClosePrice, PositionType, Margin
FROM Positions
WHERE TestId = ?
AND YEAR(Created) >= {no_results_before_year_filter}
ORDER BY Created
'''

df_positions = pd.read_sql(query_positions, conn, params=[testId])

# Fetch Positions data into a DataFrame
df_positions = pd.read_sql(query_positions, conn, params=[testId])

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
    color = 'gray'  # Default color to avoid NameError
    if row['PositionType'] == 1 and row['ClosePrice'] > row['EntryPrice']:
     color = 'pink'
    elif row['PositionType'] == 1 and row['ClosePrice'] < row['EntryPrice']:
     color = 'blue'
    elif row['PositionType'] == 2 and row['ClosePrice'] < row['EntryPrice']:
     color = 'pink'
    elif row['PositionType'] == 2 and row['ClosePrice'] > row['EntryPrice']:
     color = 'blue'

    tradeType = 'Buy' 
    if row['PositionType'] == 2:
      tradeType = 'Sell'

    fig.add_trace(go.Scatter(
        x=[row['Created'], row['ClosedAt']],
        y=[row['EntryPrice'], row['ClosePrice']],
        mode='lines+markers',
        line=dict(color=color),
        name=f"{tradeType}{', '}{f'£{locale.format_string("%.2f", row['Margin'], grouping=True)}'}{', '}{row['Id']}"
    ))

# Add title and labels
formatted_amount = f'£{locale.format_string("%.2f", df_positions['Margin'].sum(), grouping=True)}'

fig.update_layout(
    title='Test ID: ' + str(testId) + ', Instrument ID: ' + str(insId)
    + ', Margin Sum: ' + formatted_amount,
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
