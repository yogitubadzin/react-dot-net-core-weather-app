import React, { Component } from 'react';

export class Cities extends Component {
  static displayName = Cities.name;

  constructor(props) {
    super(props);
    this.state = { cities: [], loading: true };
  }

  componentDidMount() {
    this.populateCities();
  }

  static renderCitiesTable(cities) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>City</th>
            <th>Country</th>
          </tr>
        </thead>
        <tbody>
          {cities.map(city =>
            <tr key={city.Name}>
              <td>{city.cityName}</td>
              <td>{city.countryName}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Cities.renderCitiesTable(this.state.cities);

    return (
      <div>
        {contents}
      </div>
    );
  }

  async populateCities() {
    const response = await fetch('cities');
    const data = await response.json();
    this.setState({ cities: data, loading: false });
  }
}
